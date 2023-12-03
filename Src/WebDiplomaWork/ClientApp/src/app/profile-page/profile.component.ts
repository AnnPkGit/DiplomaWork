import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountModel } from '../shared/models/accountModel';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { UserResponse } from '../identification/signIn/signIn.component';
import { ImageItem } from '../toast-modal/toast-modal';
import { LocalRouter } from '../shared/localRouter/local-router.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
})
export class ProfilePageComponent implements OnInit{
  account?: AccountModel;
  modalOpened: boolean = false;
  public user: UserObject | undefined;
  public userCurrent: UserResponse | undefined;
  toastResponse: ToastResponse = {} as ToastResponse;
  currentUserId: string = '';
  toastsSelected: boolean = true;
  repliesSelected: boolean = false;
  likesSelected: boolean = false;
  mediaSelected: boolean = false;
  pageEndWasReached = false;
  youFollow: boolean = false;
  openFollowExplorer: boolean = false;
  openFollowExplorerType: string = '';

  followsOrFollowers: UserFollowResponse | any;

  @Output() reToastWasRemoved = new EventEmitter<number>();

  constructor(private httpClient: HttpClient, private route: ActivatedRoute, private router: Router, private localRouter: LocalRouter) {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.ngOnInit();
      }
    });
  }

  goBack() {
    this.openFollowExplorer = false;
    this.openFollowExplorerType = '';
  }

  ReToastRemoved(id: number) {
    console.log("ReToastRemoved")
    this.reToastWasRemoved.emit(id);
  }

  goToFollowExplorer(type: string) {
    this.followsOrFollowers = null; 
    if(type == 'Following') {
      this.fetchUsersFollows();
      this.openFollowExplorerType = 'FOLLOWS'
    }
    if(type == 'Followers') {
      this.fetchUsersFollowers();
      this.openFollowExplorerType = 'FOLLOWERS'
    }
    if(this.openFollowExplorerType == '') {
      return;
    }

    this.openFollowExplorer = true;
    // this.localRouter.goToFollowExplorer(type);
  }

  formatDate(): string {
    const currentDate = new Date();
    const date = new Date(this.user?.created ?? new Date());
  
    const options: Intl.DateTimeFormatOptions = {
      month: 'long',
      day: 'numeric',
      year: 'numeric',
    };
  
    if (currentDate.getFullYear() === date.getFullYear()) {
      // If the year is the same as the current year
      return date.toLocaleDateString('en-US', { month: 'long', day: 'numeric' });
    } else {
      // If the year is different from the current year
      return date.toLocaleDateString('en-US', options);
    }
  }

  selectReplies() {
    this.repliesSelected = true;
    this.toastsSelected = false;
    this.likesSelected = false;
    this.mediaSelected = false;
  }

  selectMedia() {
    this.mediaSelected = true;
    this.likesSelected = false;
    this.toastsSelected = false;
    this.repliesSelected = false
  }

  selectToasts() {
    this.toastsSelected = true;
    this.repliesSelected = false
    this.likesSelected = false;
    this.mediaSelected = false;
  }

  selectLikes() {
    this.likesSelected = true;
    this.toastsSelected = false;
    this.repliesSelected = false
    this.mediaSelected = false;
  }

  ngOnInit(): void {
    window.addEventListener('scroll', this.checkScroll.bind(this));
    const urlSegments = this.route.snapshot.url;
    const lastSegment = urlSegments[urlSegments.length - 1].path;
    this.currentUserId = lastSegment;

    this.userCurrent = JSON.parse(localStorage.getItem("userInfo") ?? "");
    
    this.fetchUSersToasts(); 
    this.fetchAccountInfo(); 
  }

  checkScroll() {
    var scrollPosition = window.scrollY || window.pageYOffset || document.documentElement.scrollTop;

    var totalHeight = document.documentElement.scrollHeight;

    var windowHeight = window.innerHeight;

    if (totalHeight - scrollPosition <= windowHeight + 10 && !this.pageEndWasReached) {
        this.pageEndWasReached = true;
        console.log("Reached the end of the page!");

        this.fetchNewToasts();
    }
  }

  Follow(id: string) {
    console.log(id)
    const body = {
      AccountId: id
    };
    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });

    this.httpClient.post("/api/v1/follow", body, { headers }).subscribe(
      () => {
        this.youFollow = true;
      }
      ,
      (error) => {
      }
      );
  }

  UnFollow(id: string) {
    console.log(id)
    const body = {
      FollowingId: id
    };
    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });

    this.httpClient.delete("/api/v1/follow", { body: body, headers }).subscribe(
      () => {
        this.youFollow = false;
      }
      ,
      (error) => {
      }
      );
  }

  fetchNewToasts() {
    if(this.toastResponse.hasNextPage && this.toastsSelected) {
      this.httpClient.get<ToastResponse>("api/v1/basetoast/by/account?AccountId=" +  this.currentUserId + '&pageNumber=' + (this.toastResponse.pageNumber += 1).toString())
      .subscribe((response) => {
        var newToastResponse = response;
        newToastResponse.items = this.toastResponse.items.concat(newToastResponse.items);
        this.toastResponse = newToastResponse;
      });

      setTimeout(() => {this.pageEndWasReached = false}, 2000);
    }

    if(this.toastResponse.hasNextPage && this.repliesSelected) {
      this.httpClient.get<ToastResponse>("api/v1/BaseToast/replies/by/account?AccountId=" +  this.currentUserId + '&pageNumber=' + (this.toastResponse.pageNumber += 1).toString())
      .subscribe((response) => {
        var newToastResponse = response;
        newToastResponse.items = this.toastResponse.items.concat(newToastResponse.items);
        this.toastResponse = newToastResponse;
      });

      setTimeout(() => {this.pageEndWasReached = false}, 2000);
    }

    if(this.toastResponse.hasNextPage && this.likesSelected) {
      this.httpClient.get<ToastResponse>("api/v1/BaseToast/marked/by/account?AccountId=" +  this.currentUserId + '&pageNumber=' + (this.toastResponse.pageNumber += 1).toString())
      .subscribe((response) => {
        var newToastResponse = response;
        newToastResponse.items = this.toastResponse.items.concat(newToastResponse.items);
        this.toastResponse = newToastResponse;
      });

      setTimeout(() => {this.pageEndWasReached = false}, 2000);
    }

    if(this.toastResponse.hasNextPage && this.mediaSelected) {
      this.httpClient.get<ToastResponse>("api/v1/BaseToast/withMediaItems/by/account?AccountId=" +  this.currentUserId + '&pageNumber=' + (this.toastResponse.pageNumber += 1).toString())
      .subscribe((response) => {
        var newToastResponse = response;
        newToastResponse.items = this.toastResponse.items.concat(newToastResponse.items);
        this.toastResponse = newToastResponse;
      });

      setTimeout(() => {this.pageEndWasReached = false}, 2000);
    }
  }


  onBooleanEmitted(value: boolean) {
    this.modalOpened = value;
    this.ngOnInit();
  }

  Reload() {
    window.location.reload();
  }

  openProfileEditModal() {
    this.modalOpened = true;
  }

  fetchUSersfFollows() {
    this.toastResponse = {} as ToastResponse;
    this.httpClient.get<ToastResponse>("api/v1/basetoast/by/account?AccountId=" +  this.currentUserId).subscribe((response) => {
      this.toastResponse = response;
    });
    this.selectToasts();
  }

  fetchUSersToasts() {
    this.toastResponse = {} as ToastResponse;
    this.httpClient.get<ToastResponse>("api/v1/basetoast/by/account?AccountId=" +  this.currentUserId).subscribe((response) => {
      this.toastResponse = response;
    });
    this.selectToasts();
  }

  fetchAccountInfo() {
    this.httpClient.get<UserObject>("api/v1/account/by/id?id=" +  this.currentUserId).subscribe((response) => {
      this.user = response;
      this.youFollow = this?.user?.youFollow ?? false;
    });
  }

  fetchUsersMedia() {
    this.toastResponse = {} as ToastResponse;
    this.httpClient.get<ToastResponse>("api/v1/BaseToast/withMediaItems/by/account?AccountId=" +  this.currentUserId).subscribe((response) => {
      this.toastResponse = response;
    });
    this.selectMedia();     
  }

  onDelete(id : number) {
    this.toastResponse.items = this.toastResponse?.items.filter(item => item.id !== id);
  }

  toastWasRemovedByOrigReaction(id : number) {
    this.toastResponse.items = this.toastResponse.items.filter(item => {
      return item.toastWithContent?.id !== id;
    });
  }

  fetchUsersReplies() {
    this.toastResponse = {} as ToastResponse;
    this.httpClient.get<ToastResponse>("api/v1/BaseToast/replies/by/account?AccountId=" +  this.user?.id).subscribe((response) => {
      this.toastResponse = response;
    });
    this.selectReplies();
  }

  fetchUsersReactions() {
    this.toastResponse = {} as ToastResponse;
    this.httpClient.get<ToastResponse>("api/v1/BaseToast/marked/by/account?AccountId=" +  this.user?.id).subscribe((response) => {
      this.toastResponse = response;
    });
    this.selectLikes();
  }

  addToast($event : ToastItem): void {
    $event.author = this.userCurrent?.account as UserFollower ?? null;
    this.toastResponse?.items.unshift($event);
  }

  fetchUsersFollows() {
    this.httpClient.get<UserFollowResponse>("api/v1/follow/follows?AccountId=" +  this.currentUserId).subscribe((response) => {
      this.followsOrFollowers = response;
    });
  }

  fetchUsersFollowers() {
    this.httpClient.get<UserFollowResponse>("api/v1/follow/followers?AccountId=" +  this.currentUserId).subscribe((response) => {
      this.followsOrFollowers = response;
    });
  }
}

export interface UserObject {
  login: string;
  birthDate: null | string;
  name: string;
  avatar: ImageItem;
  banner: ImageItem;
  bio: null | string;
  owner: null | string;
  allToasts: null | string;
  reactions: null | string;
  mediaItems: null | string;
  deactivated: null | string;
  deactivatedById: null | string;
  deactivatedBy: null | string;
  created: string;
  lastModified: string;
  id: number;
  domainEvents: any[]; 
  followersCount: number;
  followsCount: number;
  youFollow: boolean;
}


export interface ToastResponse {
  items: ToastItem[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface ToastItem {
  id: number;
  lastModified: string;
  created: string;
  author: UserFollower | null;
  content: string;
  type: string;
  reply: string | null;
  quotedToast: ToastResponse | null;
  youReToasted: boolean;
  youReacted: boolean;
  reactionCount: number;
  reToastCount: number;
  replyCount: number;
  isReToast: boolean;
  toastWithContent: ToastItem;
  mediaItems: ImageItem[];
  thread: ToastResponse[];
}



interface UserFollowResponse {
  items: UserFollower[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface UserFollower {
  id: number;
  login: string;
  birthDate: string | null;
  name: string;
  avatar: ImageItem;
  bio: string | null;
}

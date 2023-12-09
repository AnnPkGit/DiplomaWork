import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountModel } from '../shared/models/accountModel';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ActivatedRoute, NavigationEnd, NavigationExtras, Router } from '@angular/router';
import { UserResponse } from '../identification/signIn/signIn.component';
import { ImageItem } from '../toast-modal/toast-modal';
import { LocalRouter } from '../shared/localRouter/local-router.service';
import { error } from 'console';

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
  accountExists: boolean = true;
  accountNotFound: boolean = false;

  followsOrFollowers: UserFollowResponse = {} as UserFollowResponse;

  @Output() reToastWasRemoved = new EventEmitter<number>();

  constructor(private httpClient: HttpClient, private route: ActivatedRoute, private router: Router, private localRouter: LocalRouter) {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {

        const fragment = this.router.parseUrl(this.router.url).fragment;

        if (fragment === 'follows') {
          this.fetchUsersFollows();
          this.openFollowExplorerType = 'FOLLOWS'
        } 
        if (fragment === 'followers') {
          this.fetchUsersFollowers();
          this.openFollowExplorerType = 'FOLLOWERS'
        }

        this.user = undefined;
        this.toastResponse = {} as ToastResponse;
        this.openFollowExplorer = false;
        this.ngOnInit();
      }
    });
  }

  goBack() {
    const urlWithoutFragment = this.router.url.split('#')[0];
    this.router.navigateByUrl(urlWithoutFragment);

    this.openFollowExplorer = false;
    this.openFollowExplorerType = '';
  }

  ReToastRemoved(id: number) {
    this.reToastWasRemoved.emit(id);
  }

  followLoading: boolean = true;
  goToFollowExplorer(type: string) {
    this.followsOrFollowers = {} as UserFollowResponse; 
    if(type == 'Following') {
      this.router.navigate([], { fragment: 'follows' });
      this.fetchUsersFollows();
      this.openFollowExplorerType = 'FOLLOWS'
    }
    if(type == 'Followers') {
      this.router.navigate([], { fragment: 'followers' });
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

  formatDateBirth(): string  {
    if(!this.user?.birthDate) {
      return '';
    }

    const currentDate = new Date();
    const date = new Date(this.user?.birthDate ?? new Date());

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

  onFollowUnfollowed(id: number) {
    if( this.openFollowExplorerType != 'FOLLOWERS')
    {
      this.followsOrFollowers.items = this.followsOrFollowers.items.filter(item => item.id !== id);
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
    const fragment = this.router.parseUrl(this.router.url).fragment;

    if (fragment === 'follows') {
      this.fetchUsersFollows();
      this.openFollowExplorerType = 'FOLLOWS'
      this.openFollowExplorer = true;
    } else if (fragment === 'followers') {
      this.fetchUsersFollowers();
      this.openFollowExplorerType = 'FOLLOWERS'
      this.openFollowExplorer = true;
    }

    window.addEventListener('scroll', this.checkScroll.bind(this));
    const urlSegments = this.route.snapshot.url;
    const lastSegment = urlSegments[urlSegments.length - 1].path;
    this.currentUserId = lastSegment;

    this.userCurrent = JSON.parse(localStorage.getItem("userInfo") ?? "");
    
    this.fetchAccountInfo(); 
  }

  checkScroll() {
    var scrollPosition = window.scrollY || window.pageYOffset || document.documentElement.scrollTop;

    var totalHeight = document.documentElement.scrollHeight;

    var windowHeight = window.innerHeight;

    if (totalHeight - scrollPosition <= windowHeight + 10 && !this.pageEndWasReached) {
        this.pageEndWasReached = true;

        this.fetchNewToasts();
    }
  }

  Follow(id: string) {
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
    if(!this.user) {
      return;
    }

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
    if(this.accountNotFound) {
      return;
    }

    this.httpClient.get<UserObject>("api/v1/account/by/id?id=" +  this.currentUserId).subscribe((response) => {
      this.user = response;
      this.youFollow = this?.user?.youFollow ?? false;
      this.fetchUSersToasts();
    },
    (error) => {
      this.accountExists = false;
      this.accountNotFound = true;
    }
    );
  }

  fetchUsersMedia() {
    this.toastResponse = {} as ToastResponse;
    this.httpClient.get<ToastResponse>("api/v1/BaseToast/withMediaItems/by/account?AccountId=" +  this.currentUserId).subscribe((response) => {
      this.toastResponse = response;
    },
    (error) => {
      }
    );
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
    },
    (error) => {
    }
    );
    this.selectReplies();
  }

  fetchUsersReactions() {
    this.toastResponse = {} as ToastResponse;
    this.httpClient.get<ToastResponse>("api/v1/BaseToast/marked/by/account?AccountId=" +  this.user?.id).subscribe((response) => {
      this.toastResponse = response;
    },
    (error) => {
    }
    );
    this.selectLikes();
  }

  addToast($event : ToastItem): void {
    if(this.currentUserId != this.userCurrent?.account.id.toString()) {
      return;
    }
    if($event.type != 'Toast') {
      return;
    }

    $event.author = this.userCurrent?.account as UserFollower ?? null;
    this.toastResponse?.items.unshift($event);
  }

  anyFollow(): boolean {
    if(this.followsOrFollowers?.items) {
      return this.followsOrFollowers?.items?.length > 0;
    }
    return false;
  }

  fetchUsersFollows() {
    if(this.currentUserId == '') return;

    this.followLoading = true;
    this.httpClient.get<UserFollowResponse>("api/v1/follow/follows?AccountId=" +  this.currentUserId).subscribe((response) => {
      this.followLoading = false;
      this.followsOrFollowers = response;
    },
    (error) => {
    }
    );
  }

  fetchUsersFollowers() {
    if(this.currentUserId == '') return;

    this.followLoading = true;
    this.httpClient.get<UserFollowResponse>("api/v1/follow/followers?AccountId=" +  this.currentUserId).subscribe((response) => {
      this.followLoading = false;
      this.followsOrFollowers = response;
    },
    (error) => {
    }
    );
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
  quotedToast: ToastItem | null;
  youReToasted: boolean;
  youReacted: boolean;
  replyToToast: ToastItem;
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
  youFollow?: boolean;
}

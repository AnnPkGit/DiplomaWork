import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, EventEmitter, HostListener, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastItem } from 'src/app/profile-page/profile.component';
import { LocalRouter } from 'src/app/shared/localRouter/local-router.service';
import { ImageItem } from 'src/app/toast-modal/toast-modal';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
})
export class PostComponent implements OnInit {

  @Output() onReToast: EventEmitter<ToastItem> = new EventEmitter();

  @Output() onDelete: EventEmitter<number> = new EventEmitter();

  @Output() onToastCreation: EventEmitter<ToastItem> = new EventEmitter();

  @Output() onReToastRemoved: EventEmitter<number> = new EventEmitter();

  @Output() toastWasRemovedByOrig: EventEmitter<number> = new EventEmitter();

  @Input() reToastNumberNeedToBeReduced: EventEmitter<number> | undefined;

  closeImg: boolean = true;
  currentImg: string = '';
  currentUrl: string;
  reToastOptionsOpen = false;
  optionsOptionsOpen = false;
  makeQuoteOpen = false;
  currentUserId: string;
  
  constructor(private localRouter: LocalRouter, private http: HttpClient, private route: ActivatedRoute) {
    this.currentUrl = window.location.href;

    const urlSegments = this.route.snapshot.url;
    const lastSegment = urlSegments[urlSegments.length - 1].path;
    this.currentUserId = lastSegment;
  }

  @Input()
  postBelongsToMe: boolean = false;

  @Input()
  style: string = "post";

  @Input()
  toastPage: boolean = false;

  @Input()
  toast: ToastItem | any;

  @Input()
  reToast: boolean = false;

  isReply = false;

  beautifyDate(date: Date | string | undefined): string {
    if (!date) {
        return '';
    }

    const inputDate = typeof date === 'string' ? new Date(date) : date;

    if (isNaN(inputDate.getTime())) {
        return '';
    }

    const today = new Date();

    if (inputDate.toDateString() === today.toDateString()) {
        const timeOptions: Intl.DateTimeFormatOptions = {
            hour: 'numeric',
            minute: 'numeric',
            hour12: false,
        };

        return new Intl.DateTimeFormat(undefined, timeOptions).format(inputDate);
        // Use undefined for the locale to use the user's default locale
    } else {
        const options: Intl.DateTimeFormatOptions = {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            hour: 'numeric',
            minute: 'numeric',
            hour12: false,
        };

        return new Intl.DateTimeFormat(undefined, options).format(inputDate);
        // Use undefined for the locale to use the user's default locale
    }
}

  getAvatar() : string {
    if(this.toast.toastWithContent) {
      return this.toast.toastWithContent?.author?.avatar?.url;
    }
    return this.toast.author?.avatar?.url;
  }

  ngOnInit(): void {
    this.reToastNumberNeedToBeReduced?.subscribe((id) => {
      if(this.toast.id == id) {
        this.toast.youReToasted = false;
        this.toast.reToastsCount -= 1;
      }
    });
  }

  onToastCreationAction($event: ToastItem) {
    if($event.quotedToast) {
      return;
    }
    this.onToastCreation.emit($event);
  }

  @HostListener('document:click', ['$event'])
  handlePageClick(event: MouseEvent): void {
    if(this.reToastOptionsOpen || this.optionsOptionsOpen) {
      this.reToastOptionsOpen = false;
      this.optionsOptionsOpen = false;
      event.stopPropagation();
    }
  }

  OpenQuote(id: number) {
    this.isReply = false;
    this.makeQuoteOpen = true;
  }

  OpenReply(event: Event) {
    this.isReply = true;
    event.stopPropagation();
    this.makeQuoteOpen = true;
  }

  removeReToast(event: Event, id : number) {
    event.stopPropagation();

    if(this.toast.type == 'ReToast')
    {
      this.deleteToast(event);
      return;
    }

    this.http.delete("/api/v1/ReToast?ToastWithContentId=" + id).subscribe(
      () => {
        this.toast.youReToasted = false;
        this.toast.reToastsCount -=  1;
        this.toastWasRemovedByOrig.emit(id);
    });
  }

  reToastToast(id: number) {
    const body = {
      ToastWithContentId: id
    };

    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });

    this.http.post<ToastItem>("/api/v1/ReToast", body, { headers }).subscribe(
      (toast) => {
        this.toast.reToastsCount +=  1;
        this.toast.youReToasted = true;
        this.onReToast.emit(toast);
      },
      (error) => {
      }
    );
  }

  getReplyToAuthorName(): string{
    if(this.toast.replyToToast) {
      if(this.toast.replyToToast.quotedToast?.quotedToast?.author?.name) {
        return this.toast.replyToToast?.quotedToast?.author?.name;
      }
      if(this.toast.replyToToast?.author?.name) {
        return this.toast.replyToToast?.author?.name;
      }
    }
    return 'DeletedAccount';
  }

  mediaAny(): boolean {
    if(this.toast?.mediaItems?.length == 0 && this.toast?.toastWithContent?.mediaItems == 0) {
      return false;
    }
    return true;
  }

  getMedia(): ImageItem[]  {
    return this.toast?.mediaItems?.length > 0 ? this.toast?.mediaItems : this.toast?.toastWithContent?.mediaItems;
  }

  getMediaUrls(): string[] {
    return this.getMedia().map((item: { url: any; }) => item.url);
  }

  addRepliesCount() {
    if(this.toast.toastWithContent) {
      this.toast.toastWithContent.repliesCount += 1;
      return;
    }
    this.toast.repliesCount += 1;
  }

  openReToastOptions(event: Event) {
    event.stopPropagation();
    this.reToastOptionsOpen = true;
    this.optionsOptionsOpen = false;
  }

  goToPostPage(event: Event, id: string) : void {
    if(this.reToastOptionsOpen || this.optionsOptionsOpen) {
      this.reToastOptionsOpen = false;
      this.optionsOptionsOpen = false;
      event.stopPropagation();
      return;
    }

    event.stopPropagation();
    if(this.toast?.replyToToast?.id && !this.toastPage) {
      id = this.toast?.replyToToast?.id;
    }

    this.localRouter.goToToastPage(id);
  }

  goToProfilePage(event: Event) : void {
    event.stopPropagation();
    this.localRouter.goToProfilePage(this.toast.toastWithContent?.author?.id ?? this.toast.author?.id);
  }

  goToProfilePageById(event: Event, id: string) : void {
    event.stopPropagation();
    this.localRouter.goToProfilePage(id);
  }

  youReacted() {
    if(this.toast.toastWithContent) {
      return this.toast.toastWithContent.youReacted;
    }
    return this.toast.youReacted;
  }

  youRetoasted() {
    if(this.toast.toastWithContent) {
      return this.toast.toastWithContent.youReToasted;
    }
    return this.toast?.youReToasted ?? false;
  }

  openImg(event: Event, currentImg: string) : void {
    event.stopPropagation();
    this.currentImg = currentImg;
    this.closeImg = false;
  }

  seeOptions(event: Event) {
    event.stopPropagation();
    this.optionsOptionsOpen = true;
    this.reToastOptionsOpen = false;
  }

  like(event: Event, id: string) {
    event.stopPropagation();
    const body = {
      ToastWithContentId: id
    };
    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });
    this.http.post("/api/v1/Reaction", body, { headers }).subscribe(
      () => {}
      ,
      (error) => {
      }
      );
      
    var toast = this.toast.toastWithContent ?? this.toast;
    toast.reactionsCount += 1;
    toast.youReacted = true;
  }

  undoLike(event: Event, id: string) {
    event.stopPropagation();

    const body = {
      ToastWithContentId: id
    };
    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });

    this.http.delete("/api/v1/Reaction", { body: body, headers }).subscribe(
      () => {}
      ,
      (error) => {
      }
      );
      
    var toast = this.toast.toastWithContent ?? this.toast;
    toast.reactionsCount -= 1;
    toast.youReacted = false;
  }

  deleteToast(event: Event) {
    this.optionsOptionsOpen = false;
    
    event.stopPropagation();
    const body = {
      BaseToastId: this.toast.id
    };
    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });
    

    this.http.delete("/api/v1/BaseToast", { body: body, headers }).subscribe(
      () => {
        this.onDelete.emit(this.toast.id);
        this.onReToastRemoved.emit(this.toast.toastWithContent.id);
      }
      ,
      (error) => {
      }
      );
  }

  updateCloseImg($event: any) {
    this.closeImg = $event;
  }

  currentStyle() : String {
    if(this.style == "no-corners") {
      return 'post-no-corners';
    }
    if(this.style == "comment") {
      return 'post-comment';
    }
    if(this.style == "no-corners-end") {
      return 'post-end-no-corners';
    }
    if(this.style == "quote") {
      return 'quote-post';
    }
    return "post"
  }

  onBooleanEmitted(value: boolean) {
    this.makeQuoteOpen = value;
    this.isReply = false;
  }
}

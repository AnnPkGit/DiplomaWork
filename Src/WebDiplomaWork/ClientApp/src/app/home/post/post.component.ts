import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, HostListener, Input } from '@angular/core';
import { ToastItem } from 'src/app/profile-page/profile.component';
import { LocalRouter } from 'src/app/shared/localRouter/local-router.service';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
})
export class PostComponent {
  closeImg: boolean = true;
  currentImg: string = '';
  currentUrl: string;
  reToastOptionsOpen = false;
  makeQuoteOpen = false;

  constructor(private localRouter: LocalRouter, private http: HttpClient) {
    this.currentUrl = window.location.href;
  }

  @Input()
  style: string = "post";

  @Input()
  toast: ToastItem | any;

  @Input()
  reToast: boolean = false;

  isReply = false;

  @HostListener('document:click', ['$event'])
  handlePageClick(event: MouseEvent): void {
    if(this.reToastOptionsOpen) {
      this.reToastOptionsOpen = false;
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

  reToastToast(id: number) {
    const body = {
      ToastWithContentId: id
    };

    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });

    this.http.post("/api/v1/ReToast", body, { headers }).subscribe(
      // (user: UserResponse) => {
      //   localStorage.setItem(errorKey, '');
      //   localStorage.setItem("userInfo", JSON.stringify(user));
      //   this.router.goToHome();
      // },
      // (error) => {
      //   localStorage.setItem(errorKey, 'Login error');
      //   window.location.reload();
      // }
    );
  }

  openReToastOptions(event: Event) {
    event.stopPropagation();
    this.reToastOptionsOpen = true;
  }

  goToPostPage(event: Event) : void {
    if(this.reToastOptionsOpen) {
      this.reToastOptionsOpen = false;
      event.stopPropagation();
      return;
    }

    event.stopPropagation();
    this.localRouter.goToToastPage(this.toast.toastWithContent?.id == null ? this.toast.id : this.toast.toastWithContent?.id );
    console.log(this.toast);
    console.log('go to post page');
  }

  goToProfilePage(event: Event) : void {
    event.stopPropagation();
    console.log('go to profile');
  }

  openImg(event: Event, currentImg: string) : void {
    event.stopPropagation();
    this.currentImg = currentImg;
    this.closeImg = false;
    console.log('seeImg');
  }

  seeOptions(event: Event) {
    event.stopPropagation();
    console.log('options');
  }

  like(event: Event) {
    event.stopPropagation();
    console.log('like');
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

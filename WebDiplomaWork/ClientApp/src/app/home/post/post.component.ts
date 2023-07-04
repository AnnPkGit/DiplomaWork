import { Component, Input } from '@angular/core';
import { AppConfig } from 'src/app/shared/config';
import { LocalRouter } from 'src/app/shared/localRouter/local-router.service';
import { PostModel } from 'src/app/shared/models/postModel';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
})
export class PostComponent {
  closeImg: boolean = true;
  currentImg: string = '';
  currentUrl: string;
  isSeparatePage: boolean = false;

  constructor(private localRouter: LocalRouter) {
    this.currentUrl = window.location.href;
    this.isSeparatePage = this.currentUrl.includes(AppConfig.toastEndpoint);
  }

  @Input()
  postModel: PostModel | any;

  goToPostPage(event: Event) : void {
    event.stopPropagation();
    this.localRouter.goToToastPage();
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
}

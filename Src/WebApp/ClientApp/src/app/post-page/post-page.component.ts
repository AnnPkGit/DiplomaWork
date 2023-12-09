import { Component, OnInit } from '@angular/core';
import { PostModel } from '../shared/models/postModel';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { ToastItem, ToastResponse } from '../profile-page/profile.component';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-post-page',
  templateUrl: './post-page.component.html',
  styleUrls: ['./post-page.component.css']
})
export class PostPAgeComponent implements OnInit {
  toast: ToastItem | any;
  notFound = false;
  replies: ToastResponse = {} as ToastResponse;

  input: string = '';

  constructor(private route: ActivatedRoute, private httpClient : HttpClient, private router: Router) {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.ngOnInit();
      }
    });
  }

  onDelete(id : number) {
    this.replies.items = this.replies?.items.filter(item => item.id !== id);
  }

  onDeleteMainToast(id : number) {
    this.ngOnInit();
  }

  ngOnInit(): void {
    const urlSegments = this.route.snapshot.url;
    const lastSegment = urlSegments[urlSegments.length - 1].path;

    this.httpClient.get<ToastItem>("api/v1/BaseToast/withContent/by/id?ToastWithContentId=" +  lastSegment).subscribe((response) => {
      this.toast = response;

      this.httpClient.get<ToastResponse>("/api/v1/reply/by/toast?ToastWithContentId=" +  lastSegment).subscribe((response) => {
        this.replies = response;
      });
    }      ,
    (error) => {
      this.notFound = true;
      this.toast = null;
      return;
    });
  }

  addToast($event : ToastItem): void {
    this.replies?.items.unshift($event);
  }

  goBack() {
    window.history.back();
  }

  toastReply() {
    const body = {
      ReplyToToastId: this.toast.id,
      Content: this.input,
      ToastMediaItemIds: []
    };

    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });

    this.httpClient.post<ToastItem>("/api/v1/reply", body, { headers }).subscribe(
      (reply: ToastItem) => {
        this.replies.items.unshift(reply);
        this.toast.repliesCount += 1;
        this.input = '';
      },
      (error) => {
      }
    );
  }
}

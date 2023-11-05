import { Component } from '@angular/core';
import { PostModel } from '../shared/models/postModel';
import { ActivatedRoute } from '@angular/router';
import { ToastItem, ToastResponse } from '../profile-page/profile.component';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-post-page',
  templateUrl: './post-page.component.html',
  styleUrls: ['./post-page.component.css']
})
export class PostPAgeComponent {
  toast: ToastItem | any;
  replies: ToastResponse | any;

  input: string = '';

  constructor(private route: ActivatedRoute, private httpClient : HttpClient) {
    const urlSegments = this.route.snapshot.url;
    const lastSegment = urlSegments[urlSegments.length - 1].path;

    this.httpClient.get<ToastItem>("api/v1/BaseToast/withContent/by/id?ToastWithContentId=" +  lastSegment).subscribe((response) => {
      this.toast = response;
    });
    
    this.httpClient.get<ToastResponse>("/api/v1/reply/by/toast?ToastWithContentId=" +  lastSegment).subscribe((response) => {
      this.replies = response;
    });
  }

  toastReply() {
    const body = {
      ReplyToToastId: this.toast.id,
      Content: this.input,
      MediaItemIds: []
    };

    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });

    this.httpClient.post<ToastItem>("/api/v1/reply", body, { headers }).subscribe(
      (reply: ToastItem) => {
        this.replies.items.unshift(reply);
        this.input = '';
        console.log('sasas')
      },
      (error) => {
      }
    );
  }
}

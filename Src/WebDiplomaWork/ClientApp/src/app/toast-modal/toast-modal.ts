import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastItem } from '../profile-page/profile.component';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'toast-modal',
  templateUrl: './toast-modal.html',
})
export class ToastModalComponent {
  @Output() booleanEmitter: EventEmitter<boolean> = new EventEmitter();
  close!: () => void;

  @Input()
  toast: ToastItem | any;

  @Input()
  reply: boolean = false;

  content: string = '';

  constructor(private http: HttpClient) {}

  Close() {
      this.booleanEmitter.emit(false);
  }

  PostContent() {
    var body = null;
    var url = '';

    if(this.toast && !this.reply) {
      body = {
        QuotedToastId: this.toast.id,
        Content: this.content,
        MediaItemIds: []
      };
      url = "/api/v1/quote";
    }
    if(this.toast && this.reply) {
      body = {
        ReplyToToastId: this.toast.id,
        Content: this.content,
        MediaItemIds: []
      };
      url = "/api/v1/reply";
    }
    else {
      body = {
        Context: this.content,
        MediaItemIds: []
      };
      url = "/api/v1/toast";
    }

    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });

    this.http.post(url, body, { headers }).subscribe(
      (res) => {
        this.Close();
      },
      (error) => {
      }
    );
  }
}
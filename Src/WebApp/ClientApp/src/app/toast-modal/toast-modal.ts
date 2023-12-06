import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastItem } from '../profile-page/profile.component';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserResponse } from '../identification/signIn/signIn.component';

@Component({
  selector: 'toast-modal',
  templateUrl: './toast-modal.html',
})
export class ToastModalComponent implements OnInit {
  @Output() onToastCreation: EventEmitter<ToastItem> = new EventEmitter();

  @Output() booleanEmitter: EventEmitter<boolean> = new EventEmitter();
  close!: () => void;

  @Output() replyEmitter = new EventEmitter<void>();

  @Input()
  toast: ToastItem | any;

  @Input()
  reply: boolean = false;

  content: string = '';

  loading: boolean = false;

  ImageItems: ImageItem[] = [];

  constructor(private http: HttpClient) {}

  user: UserResponse| undefined;
  ngOnInit(): void { 
    this.user = JSON.parse(localStorage.getItem("userInfo") ?? "");
  }


  Close() {
      this.booleanEmitter.emit(false);
  }

  selectedFiles: File[] = [];

  onFileChange(event: any): void {
    const files: FileList = event.target.files;
    
    this.selectedFiles = [];

    for (let i = 0; i < Math.min(files.length, 4); i++) {
      this.selectedFiles.push(files[i]);
    }
    this.handleFiles();
  }

  handleFiles(): void {
    const files = this.selectedFiles;

    if (files && files.length > 0) {
      for (let i = 0; i < files.length; i++) {
        const file = files[i];
        this.uploadFile(file);
      }
    }
  }

  uploadFile(file: File): void {
    const formData = new FormData();
    formData.append('file', file);

    this.http.post<ImageItem>('/api/v1/MediaItem/toast', formData).subscribe(
      (response) => {
        this.ImageItems.push(response);
      },
      (error) => {
        console.error('Error uploading file:', error);
      }
    );
  }

  PostContent() {
    var body = null;
    var url = '';
    var idsOnly: number[] = this.ImageItems.map(item => item.id)

    if(this.toast && !this.reply) {
      body = {
        QuotedToastId: this.toast.id,
        Content: this.content,
        ToastMediaItemIds: idsOnly
      };
      url = "/api/v1/quote";
    }
    if(this.toast && this.reply) {
      body = {
        ReplyToToastId: this.toast.id,
        Content: this.content,
        ToastMediaItemIds: idsOnly
      };
      url = "/api/v1/reply";
      this.replyEmitter.emit();
    }
    if(!this.toast && !this.reply) {
      body = {
        Context: this.content,
        ToastMediaItemIds: idsOnly
      };
      url = "/api/v1/toast";
    }

    this.loading = true;

    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });

    this.http.post<ToastItem>(url, body, { headers }).subscribe(
      (res: ToastItem) => {
        if(res) {
          this.onToastCreation.emit(res);
        }
        this.loading = false;
        this.Close();
      },
      (error) => {
        this.loading = false;
      }
    );
  }
}

export interface ImageItem {
  id: number;
  url: string;
}
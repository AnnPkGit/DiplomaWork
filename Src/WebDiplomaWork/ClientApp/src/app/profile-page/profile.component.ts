import { Component, OnInit } from '@angular/core';
import { AccountModel } from '../shared/models/accountModel';
import { UserResponse } from '../identification/signIn/signIn.component';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
})
export class ProfilePageComponent implements OnInit{
  account?: AccountModel;
  modalOpened: boolean = false;
  public user: UserResponse | undefined;
  toastResponse: ToastResponse | undefined;

  constructor(private httpClient: HttpClient) {

  }

  ngOnInit(): void {
    this.user = JSON.parse(localStorage.getItem("userInfo") ?? "");
    this.fetchUSersToasts(); 
  }

  onBooleanEmitted(value: boolean) {
    this.modalOpened = value;
  }

  Reload() {
    window.location.reload();
  }

  openProfileEditModal() {
    this.modalOpened = true;
  }

  fetchUSersToasts() {
    this.httpClient.get<ToastResponse>("api/v1/basetoast/by/account?AccountId=" +  this.user?.account.id).subscribe((response) => {
      this.toastResponse = response;
    });
  }

  fetchUsersReplies() {
    this.httpClient.get<ToastResponse>("api/v1/basetoast/by/account?AccountId=" +  this.user?.account.id).subscribe((response) => {
      this.toastResponse = response;
    });
  }

  addToast($event : ToastItem): void {
    $event.author = this.toastResponse?.items[0].author ?? null;
    this.toastResponse?.items.unshift($event);
  }
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
  author: {} | null;
  content: string;
  type: string;
  reply: string | null;
  quotedToast: ToastResponse | null;
  reactionCount: number;
  reToastCount: number;
  replyCount: number;
  isReToast: boolean;
  toastWithContent: ToastResponse;
  mediaItems: string[];
  thread: ToastResponse[];
}

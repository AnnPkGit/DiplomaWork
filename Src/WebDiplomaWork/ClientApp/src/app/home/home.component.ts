import { Component, OnInit } from '@angular/core';
import { LocalRouter } from '../shared/localRouter/local-router.service';
import { PostModel } from '../shared/models/postModel';
import { HttpClient } from '@angular/common/http';
import { ToastResponse } from '../profile-page/profile.component';
import { UserResponse } from '../identification/signIn/signIn.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit{
  constructor(private router: LocalRouter, private httpClient: HttpClient) {}
  posts: ToastResponse = {} as ToastResponse;
  public user: UserResponse | undefined;

  ngOnInit(): void {
    this.user = JSON.parse(localStorage.getItem("userInfo") ?? "");
    this.fetchUSersToasts();
  }

  fetchUSersToasts() {
    this.httpClient.get<ToastResponse>("api/v1/toast/all").subscribe((response) => {
      this.posts = response;
    });
  }

  Reload() {
    window.location.reload();
  }

  onDelete(id : number) {
    this.posts.items = this.posts?.items.filter(item => item.id !== id);
  }
}

import { Component, OnInit } from '@angular/core';
import { LocalRouter } from '../shared/localRouter/local-router.service';
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
  pageEndWasReached = false;

  ngOnInit(): void {
    window.addEventListener('scroll', this.checkScroll.bind(this));
    this.user = JSON.parse(localStorage.getItem("userInfo") ?? "");
    this.fetchUSersToasts();
  }

  fetchUSersToasts() {
    this.httpClient.get<ToastResponse>("api/v1/BaseToast/from/follows").subscribe((response) => {
      this.posts = response;
    });
  }

  Reload() {
    window.location.reload();
  }

  onDelete(id : number) {
    this.posts.items = this.posts?.items.filter(item => item.id !== id);
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

  fetchNewToasts() {
    if(this.posts.hasNextPage) {
      this.httpClient.get<ToastResponse>("api/v1/BaseToast/from/follows" + '?&pageNumber=' + (this.posts.pageNumber += 1).toString())
      .subscribe((response) => {
        var newToastResponse = response;
        newToastResponse.items = this.posts.items.concat(newToastResponse.items);
        this.posts = newToastResponse;
      });

      setTimeout(() => {this.pageEndWasReached = false}, 2000);
    }
  }
}

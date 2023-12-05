import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { UserFollower } from '../profile-page/profile.component';
import { LocalRouter } from '../shared/localRouter/local-router.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserResponse } from '../identification/signIn/signIn.component';

@Component({
  selector: 'explore-prof-component',
  templateUrl: './explore-prof-component.html',
  styleUrls: ['./explore-prof-component.css']
})
export class ExploreProfComponent implements OnInit {

  @Output() onUnFollowed: EventEmitter<number> = new EventEmitter();

  @Input()
  user: UserFollower | any;
  userCurrent: UserResponse = {} as UserResponse;

  constructor(private localRouter: LocalRouter, private httpClient: HttpClient) {
  }

  Follow(event: Event) {
    this.user.youFollow = true;

    event.stopPropagation();
    const body = {
      AccountId: this.user.id
    };
    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });

    this.httpClient.post("/api/v1/follow", body, { headers }).subscribe(
      () => {
      }
      ,
      (error) => {
      }
      );
  }

  UnFollow(event: Event) {
    this.user.youFollow = false;
    this.onUnFollowed?.emit(this.user.id);

    event.stopPropagation();
    const body = {
      FollowingId: this.user.id
    };
    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });

    this.httpClient.delete("/api/v1/follow", { body: body, headers }).subscribe(
      () => {
      }
      ,
      (error) => {
      }
      );
  }

  ngOnInit(): void {
    this.userCurrent = JSON.parse(localStorage.getItem("userInfo") ?? "");
  }
    
  goToProfilePage() : void {
    this.localRouter.goToProfilePage(this.user.id);
  }
}
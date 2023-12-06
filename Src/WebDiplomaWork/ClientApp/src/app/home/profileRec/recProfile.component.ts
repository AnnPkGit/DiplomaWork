import { Component, OnInit } from '@angular/core';
import { FollowRecommendationsService } from 'src/app/FollowRecommendationsService/FollowRecommendationsService';
import { UserFollower } from 'src/app/profile-page/profile.component';
import { LocalRouter } from 'src/app/shared/localRouter/local-router.service';

@Component({
  selector: 'app-rec-profile',
  templateUrl: './recProfile.component.html',
})
export class RecProfileComponent implements OnInit {
  recommendations: UserFollower[] = [];

  constructor(private followRecommendationsService: FollowRecommendationsService, private localRouter: LocalRouter) {}

  ngOnInit(): void {
    this.recommendations = this.followRecommendationsService.getRecommendations();
    this.followRecommendationsService.getRecommendationsObservable().subscribe((data) => {
      this.recommendations = data;
    });
  }

  AnyItems(): boolean {
    if(this.recommendations) {
      return this.recommendations?.length > 0;
    }
    return false;
  }

  Follow(id: number, event: Event) {
    console.log(id)
    event.stopPropagation();
    this.followRecommendationsService.Follow(id);
  }

  UnFollow(id: number, event: Event) {
    console.log(id)
    event.stopPropagation();
    this.followRecommendationsService.UnFollow(id);
  }

  goToProfilePage(id: number) : void {
    this.localRouter.goToProfilePage(id.toString());
  }
}
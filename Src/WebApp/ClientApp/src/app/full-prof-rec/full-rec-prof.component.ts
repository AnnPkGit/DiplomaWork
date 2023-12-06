import { Component, OnInit } from '@angular/core';
import { UserFollower } from '../profile-page/profile.component';
import { FollowRecommendationsService } from '../FollowRecommendationsService/FollowRecommendationsService';
import { LocalRouter } from '../shared/localRouter/local-router.service';

@Component({
  selector: 'app-full-rec-prof',
  templateUrl: './full-rec-prof.component.html',
  styleUrls: ['./full-rec-prof.component.css']
})
export class FullProfRecComponent implements OnInit{

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
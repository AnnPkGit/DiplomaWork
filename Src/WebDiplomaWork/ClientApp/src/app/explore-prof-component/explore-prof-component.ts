import { Component, Input, OnInit } from '@angular/core';
import { UserFollower } from '../profile-page/profile.component';
import { LocalRouter } from '../shared/localRouter/local-router.service';

@Component({
  selector: 'explore-prof-component',
  templateUrl: './explore-prof-component.html',
  styleUrls: ['./explore-prof-component.css']
})
export class ExploreProfComponent implements OnInit {

  @Input()
  user: UserFollower | any;

  @Input()
  showFollowBtn: boolean = true;


  constructor(private localRouter: LocalRouter) {

  }

  ngOnInit(): void {
        // throw new Error('Method not implemented.');
  }
    
  goToProfilePage() : void {
    this.localRouter.goToProfilePage(this.user.id);
  }
}
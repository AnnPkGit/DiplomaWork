import { Component } from '@angular/core';
import { UserResponse } from 'src/app/identification/signIn/signIn.component';
import { NotificationService, ReactionNotification } from 'src/app/service/notifications.service';
import { LocalRouter } from 'src/app/shared/localRouter/local-router.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-navBar',
  templateUrl: './navBar.component.html',
})
export class NavBarComponent {

  lastUpdated: string | undefined;
  receivedData: ReactionNotification | any;
  userCurrent: UserResponse;
  newNotification: boolean = false;

  constructor(private router: LocalRouter, private notService: NotificationService) {
    this.userCurrent = JSON.parse(localStorage.getItem("userInfo") ?? "");
  }

  reactionNotification: ReactionNotification | undefined;
  private reactionNotificationSubscription: Subscription | undefined;

  ngOnInit(): void {
    // UNDO COMMENT TO START HUB CONNECTION

    // this.reactionNotificationSubscription = this.notService.reactionNotification$.subscribe((data) => {
    //   this.reactionNotification = data;
    // });
  }

  goToMessages() {
    this.router.goToMessages();
  }

  goToHome() {
    this.router.goToHome();
  }

  goToProfile() {
    this.router.goToProfilePage(this.userCurrent?.account?.id.toString());
  }

  goToNotifications() {
    this.router.goToNotifications();
  }
}
import { Component, HostListener } from '@angular/core';
import { UserResponse } from 'src/app/identification/signIn/signIn.component';
import { NotificationService, ReactionNotification } from 'src/app/service/notifications.service';
import { LocalRouter } from 'src/app/shared/localRouter/local-router.service';
import { Subscription } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-navBar',
  templateUrl: './navBar.component.html',
})
export class NavBarComponent {

  lastUpdated: string | undefined;
  receivedData: ReactionNotification | any;
  userCurrent: UserResponse;
  newNotification: boolean = false;
  moreOpen: boolean = false;

  constructor(private router: LocalRouter, private notService: NotificationService, private httpCLient: HttpClient) {
    this.userCurrent = JSON.parse(localStorage.getItem("userInfo") ?? "");
  }

  private reactionNotificationSubscription: Subscription | undefined;

  @HostListener('document:click', ['$event'])
  handlePageClick(event: MouseEvent): void {
    if(this.moreOpen) {
      this.moreOpen = false;
      event.stopPropagation();
    }
  }

  MoreOpen(event: Event) {
    this.moreOpen = true;
    event.stopPropagation();
  }
  
  ngOnInit(): void {
    this.newNotification = this.notService.getCurrentNotStatus();

    this.reactionNotificationSubscription = this.notService.getNotsStatus().subscribe((data) => {
      this.newNotification = data;
    });
  }

  logOut() {
    this.httpCLient.get('api/v1/auth/logout').subscribe((response) => {
        this.router.goToSignIn();
      },
      (error) => {

      });
  }

  goToSettings() {
    this.router.goToSettings();
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

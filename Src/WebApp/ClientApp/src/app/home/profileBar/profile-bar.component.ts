import { Component, OnInit } from "@angular/core";
import { UserResponse } from "src/app/identification/signIn/signIn.component";
import { LocalRouter } from "src/app/shared/localRouter/local-router.service";

@Component({
  selector: 'profile-bar',
  templateUrl: './profile-bar.component.html',
})
export class ProfileBarComponent implements OnInit{
  
  constructor(private localRouter: LocalRouter) {}

  public user: UserResponse | undefined;
  ngOnInit(): void { 
    this.user = JSON.parse(localStorage.getItem("userInfo") ?? "");
  }

  goToProfilePage() : void {
    if(this.user?.account?.id) {
      this.localRouter.goToProfilePage(this.user?.account?.id?.toString());
    }
  }
}
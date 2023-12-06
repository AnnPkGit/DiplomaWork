import { Component, OnInit } from "@angular/core";
import { UserResponse } from "src/app/identification/signIn/signIn.component";

@Component({
  selector: 'profile-bar',
  templateUrl: './profile-bar.component.html',
})
export class ProfileBarComponent implements OnInit{
  
  public user: UserResponse | undefined;
  ngOnInit(): void { 
    this.user = JSON.parse(localStorage.getItem("userInfo") ?? "");
  }
}
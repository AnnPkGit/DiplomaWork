import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { LocalRouter } from '../shared/localRouter/local-router.service';

@Component({
  selector: 'settings-page',
  templateUrl: './settings-page.html',
})
export class SettingsPageComponent {
  passwordTab: boolean = true;
  emailTab: boolean = false;

  password: string = '';
  newPassword: string = '';

  constructor(private localRouter: LocalRouter, private http: HttpClient) {
  }

  choseEmail() {
    this.passwordTab = false;
    this.emailTab = true;
  }

  chosePassword() {
    this.passwordTab = true;
    this.emailTab = false;
  }


  updatePassword() {
    const body = {
      OldPassword: this.password,
      NewPassword: this.newPassword,
    };
    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });
    this.http.patch("/api/v1/user/password", body, { headers }).subscribe(
      () => {
        this.localRouter.goToSignIn();
      }
      ,
      (error) => {
      }
      );
  }
}
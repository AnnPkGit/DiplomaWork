import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { LocalRouter } from '../shared/localRouter/local-router.service';
import { UserResponse } from '../identification/signIn/signIn.component';

@Component({
  selector: 'settings-page',
  templateUrl: './settings-page.html',
})
export class SettingsPageComponent implements OnInit {
  userCurrent: UserResponse = {} as UserResponse;

  passwordTab: boolean = true;
  emailTab: boolean = false;
  email: string = '';
  emailSending: boolean = false;
  emailSent: boolean = false;
  phone: string = '';
  phoneSending: boolean = false;
  phoneSent: boolean = false;

  password: string = '';
  newPassword: string = '';

  constructor(private localRouter: LocalRouter, private http: HttpClient) {

  }

  ngOnInit(): void {
    this.http.get<UserResponse>("api/v1/user").subscribe((response) => {
      this.userCurrent = response;
      this.email = this.userCurrent.email ?? ''; 
      this.phone = this.userCurrent.phone ?? '';
    },
    (error) => {
      }
    );
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

  updateEmail() {
    this.emailSending = true;
    const body = {
      NewEmail: this.email
    };
    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });
    this.http.patch("/api/v1/user/email", body, { headers }).subscribe(
      () => {
        this.emailSent = true;
        this.emailSending = false;
        this.userCurrent.email = this.email;
      }
      ,
      (error) => {
        this.emailSending = false;
      }
      );
  }

  updatePhone() {
    this.phoneSending = true;
    const body = {
      NewPhone: this.phone
    };
    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });
    this.http.patch("/api/v1/user/phone", body, { headers }).subscribe(
      () => {
        this.phoneSent = true;
        this.phoneSending = false;
        this.userCurrent.phone = this.phone;
      }
      ,
      (error) => {
        this.phoneSending = false;
      }
      );
  }
}
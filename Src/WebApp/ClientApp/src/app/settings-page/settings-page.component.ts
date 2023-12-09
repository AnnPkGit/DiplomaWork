import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
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
  phoneRedactorMode: boolean = false;
  phoneVerifying: boolean = false;
  phoneCodeSent: boolean = false;
  code: string = '';
  codeSending: boolean = false;

  password: string = '';
  newPassword: string = '';
  rePassword: string = '';
  passwordError: string | undefined;
  emailRedactorMode: boolean = false;
  emailVerifying: boolean = false;

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

  comparePasswords() {
    if(this.rePassword.trim() != '') {
      if(this.rePassword != this.newPassword) {
        this.passwordError = `Passwords don't match`
      }
      else {
        this.passwordError = undefined;
      }
    }
    if(this.rePassword.trim() == '' && this.newPassword.trim() =='') {
      this.passwordError = undefined;
    }
  }

  choseEmail() {
    this.passwordTab = false;
    this.emailTab = true;
    this.password = '';
    this.newPassword = '';
    this.passwordError = undefined;
  }

  chosePassword() {
    this.passwordTab = true;
    this.emailTab = false;
  }

  sendCode() {
    this.codeSending = true;
    const body = {
      VerificationCode: this.code
    };
    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });
    this.http.post("/api/v1/auth/confirm/phone", body, { headers }).subscribe(
      () => {
        this.userCurrent.phoneVerified = true;
        this.codeSending = false;
      }
      ,
      (error: HttpErrorResponse) => {
        this.codeSending = false;
      }
    );
  }

  updatePassword() {
    if(this.passwordError) {
      return;
    }

    if(this.newPassword.trim() == '' || this.rePassword.trim() == '' || this.password.trim() == '') {
      this.passwordError = 'All fields are required'
      return;
    }

    this.passwordError = undefined;

    const body = {
      OldPassword: this.password.trim(),
      NewPassword: this.newPassword.trim(),
    };
    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });
    this.http.patch("/api/v1/user/password", body, { headers }).subscribe(
      () => {
        this.passwordError = undefined;
        this.localRouter.goToSignIn();
      }
      ,
      (error: HttpErrorResponse) => {
          const validationErrors = error.error.errors;
          const newPasswordErrors = validationErrors['NewPassword'];
          if(newPasswordErrors[0]) {
            this.passwordError = newPasswordErrors[0];
          }
        }
      );
  }

  updateEmail() {
    if(!this.emailRedactorMode) {
      this.emailRedactorMode = true;
      return;
    }

    if(this.userCurrent.email == this.email.trim()) {
      return;        
    }

    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if(!emailRegex.test(this.email)) {
      return;
    }

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
        this.emailRedactorMode = false;
        this.userCurrent.emailVerified = false;
      }
      ,
      (error) => {
        this.emailSending = false;
      }
      );
  }

  updatePhone() {
    this.phoneCodeSent = false;
    
    if(!this.phoneRedactorMode) {
      this.phoneRedactorMode = true;
      return;
    }

    if(this.phone == this.userCurrent.phone?.trim()) {
      return;
    }

    this.phoneSending = true;
    const body = {
      NewPhone: this.phone
    };
    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });
    this.http.patch("/api/v1/user/phone", body, { headers }).subscribe(
      () => {
        this.userCurrent.phoneVerified = false;
        this.phoneSent = true;
        this.phoneSending = false;
        this.userCurrent.phone = this.phone;
        this.phoneRedactorMode = false;
      }
      ,
      (error) => {
        this.phoneSending = false;
      }
      );
  }

  canVerifyPhone() : boolean {
    if(this.userCurrent.phone && !this.userCurrent.phoneVerified && !this.phoneRedactorMode) {
      return true;
    }
    return false;
  }


  canVerifyEmail() : boolean {
    if(this.userCurrent.email && !this.userCurrent.emailVerified && !this.emailRedactorMode) {
      return true;
    }
    return false;
  }

  verifyEmail() {
    this.emailVerifying = true;
  
    this.http.get("/api/v1/auth/send/email").subscribe(
      () => {
        this.emailVerifying = false;
      }
      ,
      (error) => {
        this.emailVerifying = false;
      }
      );
  }

  verifyPhone() {
    this.phoneVerifying = true;

    this.http.post("/api/v1/auth/send/sms", null).subscribe(
      () => {
        this.phoneVerifying = false;
        this. phoneCodeSent = true;
      }
      ,
      (error) => {
        this.phoneVerifying = false;
      }
      );
  }
}
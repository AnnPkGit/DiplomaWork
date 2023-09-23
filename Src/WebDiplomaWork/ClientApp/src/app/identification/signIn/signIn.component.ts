import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { LocalRouter } from 'src/app/shared/localRouter/local-router.service';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'signIn-step',
  templateUrl: './signIn.component.html',
})
export class SignInComponent {
  password: string = '';
  email: string = '';

  emailError: string = '';
  passwordError: string = '';

  constructor(private router: LocalRouter, private auth: AuthService, private http: HttpClient) { }

  emailValidate() {
    const regex = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/;
    if(!regex.exec(this.email))
    {
      this.emailError = 'Wrong email format';
    }
    else {
      this.emailError = '';
    }
  }

  RedirectToAuthPage() {
    this.router.goToAuth();
  }

  Authorize() {
    if(this.password.trim() === '') {
      this.passwordError = 'Password is required';
      return;
    }
    else {
      this.passwordError = '';
    }

    const body = {
      Password: "this",
      Email: "emailInput"
    };

    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });

    this.http.post('api/v1/auth/login', body, { headers }).subscribe(
      () => {
        console.log('auth successful');
      },
      (error) => {
        console.error('auth Error during registration:', error);
      }
    );

    //допилить
    this.auth.Authorize();
    this.router.goToHome();
  }
}

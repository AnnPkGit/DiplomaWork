import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NotificationService } from 'src/app/service/notifications.service';
import { ServerEndpoint } from 'src/app/shared/config';
import { LocalRouter } from 'src/app/shared/localRouter/local-router.service';
import { AuthService } from 'src/app/shared/services/auth.service';
import { ImageItem } from 'src/app/toast-modal/toast-modal';

const errorKey: string = 'error';

@Component({
  selector: 'signIn-step',
  templateUrl: './signIn.component.html',
})
export class SignInComponent implements OnInit {
  password: string = '';
  email: string = '';

  emailError: string = '';
  passwordError: string = '';
  loginError: string | null = '';

  requestInProcess: boolean = false;

  constructor(private router: LocalRouter, private auth: AuthService, private http: HttpClient,private notService: NotificationService ) { }

  ngOnInit(): void {
    this.loginError = localStorage.getItem(errorKey);
    localStorage.setItem(errorKey, '');
  }

  emailValidate(): boolean {
    const regex = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/;
    const isMatch = regex.test(this.email); 
  
    if (!isMatch) {
      this.emailError = 'Wrong email format';
    } else {
      this.emailError = '';
    }
  
    return isMatch;
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
    
    if(!this.emailValidate()) {
      return;
    }

    this.requestInProcess = true;

    const body = {
      Password: this.password,
      Email: this.email
    };

    const headers = new HttpHeaders({
      'Content-Type': 'application/json' 
    });

    this.http.post<UserResponse>(ServerEndpoint.loginEndpoint, body, { headers }).subscribe(
      (user: UserResponse) => {
        localStorage.setItem(errorKey, '');
        localStorage.setItem("userInfo", JSON.stringify(user));
        this.requestInProcess = false;
        this.notService.startSignlaRConnection();
        this.router.goToHome();
      },
      (error) => {
        localStorage.setItem(errorKey, 'Login error');
        window.location.reload();
      }
    );
  }
}

export interface UserResponse {
  email: string;
  phone: string | null;
  emailVerified: boolean;
  phoneVerified: boolean;
  account: {
    id: number;
    login: string;
    birthDate: Date | null;
    name: string;
    avatar: ImageItem | null;
    bio: string | null;
    banner: ImageItem;
  };
}
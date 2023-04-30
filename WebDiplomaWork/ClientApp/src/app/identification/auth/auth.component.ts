import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
})
export class AuthComponent {
  constructor(private router: Router) {}

  RedirectToSignInPage() {
    this.router?.navigate(['/sign-in']);
  }

  RedirectToRegisterPage() {
    this.router?.navigate(['/register']);
  }
}

import { Component } from '@angular/core';
import { LocalRouter } from 'src/app/shared/localRouter/local-router.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
})
export class AuthComponent {
  constructor(private router: LocalRouter) {}

  RedirectToSignInPage() {
    this.router.goToSignIn();
  }

  RedirectToRegisterPage() {
    this.router.goToRegister();
  }
}

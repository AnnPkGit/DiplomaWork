import { Component } from '@angular/core';
import { LocalRouter } from 'src/app/shared/localRouter/local-router.service';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'signIn-step',
  templateUrl: './signIn.component.html',
})
export class SignInComponent {
  constructor(private router: LocalRouter, private auth: AuthService) { }

  RedirectToAuthPage() {
    this.router.goToAuth();
  }

  Authorize() {
    this.auth.Authorize();
    this.router.goToHome();
  }
}

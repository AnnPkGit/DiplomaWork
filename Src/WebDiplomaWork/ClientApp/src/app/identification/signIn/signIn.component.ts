import { Component } from '@angular/core';
import { LocalRouter } from 'src/app/shared/localRouter/local-router.service';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'signIn-step',
  templateUrl: './signIn.component.html',
})
export class SignInComponent {
  userIdentifierInput: string = '';
  passwordInput: string = '';
  constructor(private router: LocalRouter, private auth: AuthService) { }

  onChangeIdentifierInput(newValue: string) {
    this.userIdentifierInput = newValue;
  }

  onChangePasswordInput(newValue: string) {
    this.passwordInput = newValue;
  }
  RedirectToAuthPage() {
    this.router.goToAuth();
  }

  Authorize() {
    this.auth.Authorize({
      Email: this.userIdentifierInput,
      Password: this.passwordInput
    }).add(() => {
      this.router.goToHome()
    });
  }
}

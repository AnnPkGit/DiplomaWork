import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'signIn-step',
  templateUrl: './signIn.component.html',
})
export class SignInComponent {
  constructor(private router: Router) { }

  RedirectToAuthPage() {
    this.router?.navigate(['']);
  }
}

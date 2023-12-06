import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { AppConfig } from '../config';
import { AuthService } from '../services/auth.service';
import { CookieService } from 'ngx-cookie-service';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private auth: AuthService, private cookieService: CookieService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree {
    // if (this.cookieService.check(".AspNetCore.Session")) {
      return true; 
    // } else {
    //   return this.router.parseUrl(AppConfig.signInEndpoint);
    // }
  }
}
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { AppConfig } from '../config';

@Injectable({
    providedIn: 'root' 
  })
  export class LocalRouter {
    constructor(private router: Router) { }

    goToHome() {
        this.router?.navigate([AppConfig.homeEndpoint]);
    }
  
    goToSignIn() {
        this.router?.navigate([AppConfig.signInEndpoint]);
    }

    goToRegister() {
        this.router?.navigate([AppConfig.registerEndpoint]);
    }

    goToAuth() {
        this.router?.navigate([AppConfig.authEndpoint]);
    }

    goToMessages() {
        this.router?.navigate([AppConfig.messagesEndpoint]);
    }

    goToToastPage(id: string) {
        this.router?.navigate([AppConfig.toastEndpoint + id]);
    }

    goToProfilePage(id: string) {
        this.router?.navigate([AppConfig.profileEndpoint + id]);
    }

    goToNotifications() {
        this.router?.navigate([AppConfig.notificationsEndpoint]);
    }

    goToSettings() {
        this.router?.navigate([AppConfig.settingsEndpoint]);
    }
}
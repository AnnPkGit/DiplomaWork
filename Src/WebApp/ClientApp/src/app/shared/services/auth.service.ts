import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root' 
  })
  export class AuthService {

    Authorize() {
        localStorage.setItem('token', 'userToken');
    }

    LogOut() {
        localStorage.setItem('token', '');
    }

    IsAthorized() {
        var token = localStorage.getItem('token');
        if(token === '' || token === null ) {
            return false;
        }
        return true;
    }
}
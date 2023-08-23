import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
  })
  export class AuthService {
    constructor(private http: HttpClient) {
    }
    Authorize(requestModel: LoginRequest) {

        const headers = new HttpHeaders({
          'Content-Type': 'application/json'
        });

        return this.http.post<TokenModel>('api/v1/auth/login', requestModel, { headers }).subscribe(
          (response: TokenModel) => {
            localStorage.setItem('accessToken', response.accessToken);
            localStorage.setItem('refreshToken', response.refreshToken);

            console.log("Login successful")
          },
          (error) => {
            console.error('Error during login:', error);
          }
        );
    }

    LogOut() {
      localStorage.setItem('accessToken', '');
      localStorage.setItem('refreshToken', '');
    }

    IsAuthorized() {
        var accessToken = localStorage.getItem('accessToken');
        if(accessToken === '' || accessToken === null ) {
            return false;
        }
        return true;
    }

    GetTokens() : TokenModel | never {
      const accessToken = localStorage.getItem('accessToken');
      const refreshToken = localStorage.getItem('refreshToken');
      if(accessToken != null && refreshToken != null){
        return { accessToken: accessToken, refreshToken: refreshToken };
      }
      throw new Error("Auth error");
    }
}
interface LoginRequest {
  Email: string;
  Password: string;
}

interface TokenModel {
  accessToken: string;
  refreshToken: string;
}

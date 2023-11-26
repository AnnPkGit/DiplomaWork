import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { LocalRouter } from '../shared/localRouter/local-router.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router, private localRouter: LocalRouter) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      tap(
        (event) => {
          if (event instanceof HttpResponse) {
            // Check if the response has a 401 status code
            if (event.status === 401) {
              this.handle401Response();
            }
          }
        },
        (error) => {
          // Check if the error response has a 401 status code
          if (error.status === 401) {
            this.handle401Response();
          }
        }
      )
    );
  }

  private handle401Response(): void {
    // Your custom logic for handling 401 responses (e.g., redirect to login)
    console.log('Unauthorized request. Redirecting to login...');
    // For example, you can redirect the user to the login page
    this.localRouter.goToSignIn();
  }
}
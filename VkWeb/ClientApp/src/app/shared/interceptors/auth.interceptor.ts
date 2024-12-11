import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse
} from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { AuthService } from '@services/auth.service';


@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(
    private router: Router,
    private authService: AuthService
  ) { }

  intercept(req: HttpRequest<any>,
    next: HttpHandler): Observable<HttpEvent<any>> {

    const idToken = localStorage.getItem('auth_token');
    if (idToken) {
      const cloned = req.clone({
        headers: req.headers.set('Authorization',
          'Bearer ' + idToken)
      });

      return next.handle(cloned).pipe(
        map((event: HttpEvent<any>) => {
          //if (event instanceof HttpResponse) {
          //  console.log('event--->>>', event);
          //}
          return event;
        }),
        catchError(err => this.checkError(err)));
    } else {
      return next.handle(req).pipe(
        map((event: HttpEvent<any>) => {
          //if (event instanceof HttpResponse) {
          //  console.log('event--->>>', event);
          //}
          return event;
        }),
        catchError(err => this.checkError(err)));
    }
  }

  checkError(err: HttpErrorResponse ) {
    if (err.status === 401) {
      this.authService.logout();
      this.router.navigate(['/login']);
    }

    return throwError(err);
  }
}

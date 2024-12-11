import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from '@services/config.service';
import { BaseService } from './base.service';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs';
import '../../rxjs-operators';
import { tap, mergeMap } from 'rxjs/operators';
import { IUser } from '@models/user.interface';

@Injectable()
export class AuthService extends BaseService {

  baseUrl = '';

  // Observable navItem source
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this._authNavStatusSource.asObservable();

  private loggedIn = false;

  constructor(private http: HttpClient, private configService: ConfigService) {
    super();
    this.loggedIn = !!localStorage.getItem('auth_token');
    this._authNavStatusSource.next(this.loggedIn);
    this.baseUrl = configService.getApiURI();
  }

  register(email: string, password: string, firstName: string, lastName: string, organizationName: string): Observable<any> {
    const body = JSON.stringify({ email, password, firstName, lastName, organizationName });
    return this.http.post<IUser>(this.baseUrl + '/user', body);
  }

  acceptInvite(email: string, password: string, firstName: string, lastName: string): Observable<any> {
    const body = JSON.stringify({ email, password, firstName, lastName });
    return this.http.post<IUser>(this.baseUrl + '/user', body);
  }

  login(userName, password) {
    return this.http.post<any>(this.baseUrl + '/auth/login', JSON.stringify({ userName, password }))
      .pipe(
        tap((res) => {
          localStorage.setItem('auth_token', res.auth_token);
          this.loggedIn = true;
          this._authNavStatusSource.next(true);
        })
      ).shareReplay();
  }

  logout() {
    localStorage.removeItem('auth_token');
    this.loggedIn = false;
    this._authNavStatusSource.next(false);
  }

  isLoggedIn() {
    return this.loggedIn;
  }

  resendConfirmation(email) {
    return this.http.get<any>(this.baseUrl + `/auth/resend-confirmation-email?email=${email}`);
  }

  requestPasswordReset(email) {
    return this.http.get<any>(this.baseUrl + `/auth/request-password-reset?email=${email}`);
  }

  resetPassword(userId, token, newPassword) {
    return this.http.post<any>(this.baseUrl + '/auth/reset-password', JSON.stringify({ userId, token, newPassword }));
  }

  //facebookLogin(accessToken: string) {
  //  let headers = new Headers();
  //  headers.append('Content-Type', 'application/json');
  //  let body = JSON.stringify({ accessToken });
  //  return this.http
  //    .post(
  //      this.baseUrl + '/externalauth/facebook', body, { headers })
  //    .map(res => res.json())
  //    .map(res => {
  //      localStorage.setItem('auth_token', res.auth_token);
  //      this.loggedIn = true;
  //      this._authNavStatusSource.next(true);
  //      return true;
  //    })
  //    .catch(this.handleError);
  //}
}


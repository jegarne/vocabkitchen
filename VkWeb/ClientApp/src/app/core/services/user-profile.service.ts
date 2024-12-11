import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from '@services/config.service';
import { User } from '@models/user';

@Injectable()
export class UserProfileService {
  baseUrl = '';

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.baseUrl = configService.getApiURI();
  }

  getUser() {
    return this.http.get<User>(this.baseUrl + '/user/');
  }

  changePassword(oldPassword, newPassword) {
    return this.http.post<any>(this.baseUrl + '/user/change-password',
      JSON.stringify({ oldPassword, newPassword }));
  }
}

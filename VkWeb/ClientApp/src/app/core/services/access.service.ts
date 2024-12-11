import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from '@services/config.service';
import { Observable } from 'rxjs/Rx';
import { IUserAccess } from '@models/user-access.interface';

@Injectable()
export class AccessService {
  baseUrl: string;

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.baseUrl = configService.getApiURI();
  }

  get(): Observable<IUserAccess> {
    return this.http.get<IUserAccess>(this.baseUrl + `/auth/access`);
  }

}

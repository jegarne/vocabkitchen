import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ProfilerRequest } from '@models/profiler-request';
import { Observable } from 'rxjs';
import { ConfigService } from '@services/config.service';

@Injectable()
export class ProfilerService {
  baseUrl: string;

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.baseUrl = configService.getApiURI();
  }

  profile(request: ProfilerRequest): Observable<ProfilerRequest> {
    let body = JSON.stringify(request);
    return this.http.post<ProfilerRequest>(this.baseUrl + '/profiler/profile/', body);
  }

  public downloadFile(request: ProfilerRequest): Observable<any> {
    const httpOptions = {
      responseType: (('blob') as any) as 'json',
    };
    let body = JSON.stringify(request);
    return this.http.post(this.baseUrl + '/profiler/word-doc', body, httpOptions);

  }

}

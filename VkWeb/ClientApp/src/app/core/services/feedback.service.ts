import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ProfilerRequest } from '@models/profiler-request';
import { Observable } from 'rxjs';
import { ConfigService } from '@services/config.service';

@Injectable()
export class FeedbackService {
  baseUrl: string;

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.baseUrl = configService.getApiURI();
  }

  sendFeedback(data): Observable<any> {
    let body = JSON.stringify(data);
    return this.http.post<any>(this.baseUrl + '/feedback/', body);
  }

}

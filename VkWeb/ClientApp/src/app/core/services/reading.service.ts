import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from '@services/config.service';
import { Observable } from 'rxjs/Rx';
import { Reading } from '@models/reading';
import { Edit } from '@models/edit';
import { Definition } from '@models/reading';
import { Tag, ReadingInfo } from '@models/org';

@Injectable()
export class ReadingService {

  baseUrl: string = '';

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.baseUrl = configService.getApiURI();
  }

  get(id: string): Observable<Reading> {
    return this.http.get<Reading>(this.baseUrl + `/reading?id=${id}`);
  }

  post(reading: Reading): Observable<Reading> {
    return this.http.post<Reading>(this.baseUrl + "/reading", reading);
  }

  postFromProfiler(text: string): Observable<Reading> {
    const body = JSON.stringify({ text });
    return this.http.post<Reading>(this.baseUrl + "/reading/profiled", body);
  }

  put(readingId: string, title: string, edits: Edit[]): Observable<Reading> {
    const body = JSON.stringify({ readingId, title, edits });
    return this.http.put<Reading>(this.baseUrl + "/reading", body);
  }

  delete(id: string): Observable<any> {
    return this.http.delete(this.baseUrl + `/reading?id=${id}`);
  }

  postDefinition(reading: Definition): Observable<Reading> {
    return this.http.post<Reading>(this.baseUrl + "/reading/definition", reading);
  }

  putDefinition(annotationId: string, value: string): Observable<any> {
    const body = JSON.stringify({ annotationId, value });
    return this.http.put(this.baseUrl + "/reading/definition", body);
  }

  removeDefinition(readingId: string, contentItemId: string): Observable<Reading> {
    return this.http.delete<Reading>(this.baseUrl + `/reading/definition?readingId=${readingId}&contentItemId=${contentItemId}`);
  }

  postTag(readingId: string, tagId: string): Observable<Tag[]> {
    const body = JSON.stringify({ readingId, tagId });
    return this.http.post<Tag[]>(this.baseUrl + "/reading/tag", body);
  }

  deleteTag(readingId: string, tagId: string): Observable<Tag[]> {
    return this.http.delete<Tag[]>(this.baseUrl + `/reading/tag?id=${readingId}&tagId=${tagId}`);
  }

  mergeTags(orgId: string, tagId: string, readingIds: string[]): Observable<ReadingInfo[]> {
    const body = JSON.stringify({ readingIds, tagId, orgId });
    return this.http.post<ReadingInfo[]>(this.baseUrl + "/reading/tags", body);
  }
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from '@services/config.service';
import { Observable } from 'rxjs/Rx';
import { Org, Tag, ReadingInfo, Student, WordAttemptSummary } from '@models/org';
import { StudentProgress } from '@models/student-progress';

@Injectable()
export class OrgService {
  baseUrl: string = '';

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.baseUrl = configService.getApiURI();
  }

  list(): Observable<Org[]> {
    return this.http.get<Org[]>(this.baseUrl + "/orgs");
  }

  get(id: string): Observable<Org> {
    return this.http.get<Org>(this.baseUrl + `/org/?id=${id}`);
  }

  setDefaultOrg(orgId: string): Observable<Org[]> {
    const body = JSON.stringify({ orgId });
    return this.http.put<Org[]>(this.baseUrl + "/org/default", body);
  }

  getStudentsProgress(id: string): Observable<Student[]> {
    return this.http.get<Student[]>(this.baseUrl + `/org/students?orgId=${id}`);
  }

  getStudentProgress(orgId: string, studentId: string): Observable<StudentProgress> {
    return this.http.get<StudentProgress>(this.baseUrl + `/org/student?orgId=${orgId}&studentId=${studentId}`);
  }

  getWordsProgress(id: string): Observable<WordAttemptSummary[]> {
    return this.http.get<WordAttemptSummary[]>(this.baseUrl + `/org/words?orgId=${id}`);
  }

  addTag(orgId: string, value: string): Observable<Tag[]> {
    const body = JSON.stringify({ orgId, value });
    return this.http.post<Tag[]>(this.baseUrl + "/org/tag", body);
  }

  toggleDefaultTag(tagId: string): Observable<Tag[]> {
    const body = JSON.stringify({ id: tagId });
    return this.http.put<Tag[]>(this.baseUrl + "/org/tag/default", body);
  }

  getTag(tagId: string): Observable<Tag> {
    return this.http.get<Tag>(this.baseUrl + `/org/tag/?tagId=${tagId}`);
  }

  deleteTag(tagId: string): Observable<Tag> {
    return this.http.delete<Tag>(this.baseUrl + `/org/tag/?tagId=${tagId}`);
  }

  searchTags(orgId: string, value: string): Observable<Tag[]> {
    return this.http.get<Tag[]>(this.baseUrl + `/org/tag/search?orgId=${orgId}&value=${value}`);
  }

  getTaggedReadings(orgId: string, tagId: string): Observable<ReadingInfo[]> {
    return this.http.get<ReadingInfo[]>(this.baseUrl + `/org/tag/readings?orgId=${orgId}&tagId=${tagId}`);
  }

  getTaggedStudents(orgId: string, tagId: string): Observable<Student[]> {
    return this.http.get<Student[]>(this.baseUrl + `/org/tag/students?orgId=${orgId}&tagId=${tagId}`);
  }

}

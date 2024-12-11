import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ConfigService } from '@services/config.service';
import { Observable } from 'rxjs/Rx';
import { Student, Tag, ReadingInfo } from '@models/org';
import { StudentWord } from '@models/student-word';

@Injectable()
export class StudentService {

  baseUrl: string = '';

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.baseUrl = configService.getApiURI();
  }

  get(id: string): Observable<Student> {
    if (id)
      return this.http.get<Student>(this.baseUrl + `/student?id=${id}`);
    else
      return this.http.get<Student>(this.baseUrl + `/student`);
  }

  postTag(studentId: string, tagId: string): Observable<Tag[]> {
    const body = JSON.stringify({ studentId, tagId });
    return this.http.post<Tag[]>(this.baseUrl + "/student/tag", body);
  }

  deleteTag(studentId: string, tagId: string): Observable<Tag[]> {
    return this.http.delete<Tag[]>(this.baseUrl + `/student/tag?id=${studentId}&tagId=${tagId}`);
  }

  mergeTags(orgId: string, tagId: string, studentIds: string[]): Observable<Student[]> {
    const body = JSON.stringify({ studentIds, tagId, orgId });
    return this.http.post<Student[]>(this.baseUrl + "/student/tags", body);
  }

  getReadings(): Observable<ReadingInfo[]> {
    return this.http.get<ReadingInfo[]>(this.baseUrl + `/student/readings`);
  }

  getWords(): Observable<StudentWord[]> {
    return this.http.get<StudentWord[]>(this.baseUrl + `/student/words`);
  }

  getUnknownWords(readingId: string): Observable<StudentWord[]> {
    return this.http.get<StudentWord[]>(this.baseUrl + `/student/words-unknown?readingId=${readingId}`);
  }

  getPretestWords(readingId: string): Observable<StudentWord[]> {
    return this.http.get<StudentWord[]>(this.baseUrl + `/student/pretest?readingId=${readingId}`);
  }

  setKnownWord(word: StudentWord): Observable<StudentWord> {
    const body = JSON.stringify({ wordId: word.id, annotationId: word.annotationId });
    return this.http.post<StudentWord>(this.baseUrl + `/student/word-known`, body);
  }

  addStudentWord(word: StudentWord): Observable<StudentWord> {
    const body = JSON.stringify({ wordId: word.id, annotationId: word.annotationId });
    return this.http.post<StudentWord>(this.baseUrl + `/student/word`, body);
  }

  addWordAttempt(word: StudentWord, attemptType: string, wasSuccessful: boolean): Observable<StudentWord> {
    const body = JSON.stringify({ wordId: word.id, annotationId: word.annotationId, attemptType, wasSuccessful });
    return this.http.post<StudentWord>(this.baseUrl + `/student/word-attempt`, body);
  }

  getRandomWords(): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + `/student/words-random`);
  }
}

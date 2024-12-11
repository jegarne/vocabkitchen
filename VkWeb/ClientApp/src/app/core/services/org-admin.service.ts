import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from '@services/config.service';
import { Observable } from 'rxjs/Rx';
import { Organization } from '@models/organization';
import { OrgTeacher } from '@models/org-teacher';
import { OrgStudent } from '@models/org-student';
import { OrgInvite } from '@models/org-invite';
import { OrgUser } from '@models/org-user';

@Injectable()
export class OrgAdminService {

  baseUrl: string = '';

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.baseUrl = configService.getApiURI();
  }

  list(): Observable<Organization[]>{
    return this.http.get<Organization[]>(this.baseUrl + "/admin/orgs");
  }

  get(orgId): Observable<Organization> {
    return this.http.get<Organization>(this.baseUrl + `/admin/org?orgId=${orgId}`);
  }

  getTeachers(orgId): Observable<OrgTeacher[]> {
    return this.http.get<OrgTeacher[]>(this.baseUrl + `/admin/org/teachers?orgId=${orgId}`);
  }

  inviteTeachers(orgInvite: OrgInvite): Observable<any> {
    return this.http.post<OrgInvite>(this.baseUrl + `/admin/org/teachers`, orgInvite);
  }

  removeTeacher(orgId, teacherId): Observable<any> {
    return this.http.delete(this.baseUrl + `/admin/org/teacher?orgId=${orgId}&teacherId=${teacherId}`);
  }

  getStudents(orgId): Observable<OrgStudent[]> {
    return this.http.get<OrgStudent[]>(this.baseUrl + `/admin/org/students?orgId=${orgId}`);
  }

  inviteStudents(orgInvite: OrgInvite): Observable<any> {
    return this.http.post<OrgInvite>(this.baseUrl + `/admin/org/students`, orgInvite);
  }

  removeStudent(orgId, studentId): Observable<any> {
    return this.http.delete(this.baseUrl + `/admin/org/student?orgId=${orgId}&studentId=${studentId}`);
  }

  removeInvite(orgId, email): Observable<any> {
    return this.http.delete(this.baseUrl + `/admin/org/invite?orgId=${orgId}&email=${email}`);
  }

  addAdmin(user: OrgUser): Observable<any> {
    return this.http.post<OrgUser>(this.baseUrl + `/admin/org/admin`, user);
  }

  removeAdmin(user: OrgUser): Observable<any> {
    return this.http.delete<OrgUser>(this.baseUrl + `/admin/org/admin?orgId=${user.orgId}&userId=${user.vkUserId}`);
  }
}


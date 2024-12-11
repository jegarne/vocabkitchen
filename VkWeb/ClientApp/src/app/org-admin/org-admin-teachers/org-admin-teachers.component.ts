import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrgAdminService } from '@services/org-admin.service';
import { OrgTeacher } from '@models/org-teacher';
import { OrgInvite } from '@models/org-invite';
import { OrgUser } from '@models/org-user';

@Component({
  selector: 'org-admin-teachers',
  templateUrl: './org-admin-teachers.component.html',
  styleUrls: ['./org-admin-teachers.component.css']
})
export class OrgAdminTeachersComponent implements OnInit, OnDestroy {
  orgId: string;
  private sub: any;
  teachers: OrgTeacher[];
  serverErrors: any;
  inviteEmails: string[];

  constructor(
    private route: ActivatedRoute,
    private orgService: OrgAdminService,
  ) { }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.orgId = params['id'];
      this.orgService.getTeachers(this.orgId).subscribe(
        teachers => {
          this.teachers = teachers;
          this.serverErrors = '';
        },
        errors => {
          this.serverErrors = errors;
        });
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  removeTeacher(teacherId) {
    this.orgService.removeTeacher(this.orgId, teacherId).subscribe(
      result => this.ngOnInit(),
      errors => {
        this.serverErrors = errors;
      });
  }

  removeInvite(email) {
    this.orgService.removeInvite(this.orgId, email).subscribe(
      result => this.ngOnInit(),
      errors => {
        this.serverErrors = errors;
      });
  }

  handleUpdateEmails(emails: string[]) {
    this.inviteEmails = emails;
  }

  sendInvites() {
    this.orgService.inviteTeachers(new OrgInvite(this.orgId, this.inviteEmails)).subscribe(
      result => this.ngOnInit(),
      errors => {
        this.serverErrors = errors;
      });;
  }

  setAdmin(userId) {
    this.orgService.addAdmin(new OrgUser(this.orgId, userId)).subscribe(
      result => this.ngOnInit(),
      errors => {
        this.serverErrors = errors;
      });
    ;
  }

  removeAdmin(userId) {
    this.orgService.removeAdmin(new OrgUser(this.orgId, userId)).subscribe(
      result => this.ngOnInit(),
      errors => {
        this.serverErrors = errors;
      });
    ;
  }
}

import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrgAdminService } from '@services/org-admin.service';
import { OrgStudent } from '@models/org-student';
import { OrgInvite } from '@models/org-invite';

@Component({
  selector: 'org-admin-students',
  templateUrl: './org-admin-students.component.html',
  styleUrls: ['./org-admin-students.component.css']
})
export class OrgAdminStudentsComponent implements OnInit, OnDestroy {
  orgId: string;
  private sub: any;
  students: OrgStudent[];
  serverErrors: any;
  inviteEmails: string[];

  constructor(
    private route: ActivatedRoute,
    private orgService: OrgAdminService,
  ) { }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.orgId = params['id'];
      this.orgService.getStudents(this.orgId).subscribe(
        students => {
          this.students = students;
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

  removeStudent(id) {
    this.orgService.removeStudent(this.orgId, id).subscribe(
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
    this.orgService.inviteStudents(new OrgInvite(this.orgId, this.inviteEmails)).subscribe(
      result => this.ngOnInit(),
      errors => {
        this.serverErrors = errors;
      });;
  }
}

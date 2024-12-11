import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';

import { OrgAdminRoutingModule } from './org-admin-routing.module';
import { OrgAdminComponent } from './org-admin.component';
import { OrgAdminNavComponent } from './org-admin-nav/org-admin-nav.component';
import { OrgAdminDetailComponent } from './org-admin-detail/org-admin-detail.component';
import { OrgAdminTeachersComponent } from './org-admin-teachers/org-admin-teachers.component';
import { OrgAdminStudentsComponent } from './org-admin-students/org-admin-students.component';
import { InviteBoxComponent } from './invite-box/invite-box.component';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    OrgAdminRoutingModule,
    SharedModule
  ],
  declarations: [
    OrgAdminComponent,
    OrgAdminNavComponent,
    OrgAdminDetailComponent,
    OrgAdminTeachersComponent,
    OrgAdminStudentsComponent,
    InviteBoxComponent
  ]
})
export class OrgAdminModule { }

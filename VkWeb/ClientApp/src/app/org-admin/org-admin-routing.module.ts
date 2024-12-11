import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OrgAdminComponent } from './org-admin.component';
import { OrgAdminDetailComponent } from './org-admin-detail/org-admin-detail.component';
import { OrgAdminTeachersComponent } from './org-admin-teachers/org-admin-teachers.component';
import { OrgAdminStudentsComponent } from './org-admin-students/org-admin-students.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'select' },
  {
    path: 'select',
    component: OrgAdminComponent
  },
  {
    path: 'details/:id',
    component: OrgAdminDetailComponent
  },
  {
    path: 'teachers/:id',
    component: OrgAdminTeachersComponent
  },
  {
    path: 'students/:id',
    component: OrgAdminStudentsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrgAdminRoutingModule { }

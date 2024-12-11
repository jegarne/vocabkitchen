import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OrgDashboardComponent } from './org-dashboard.component';
import { OrgDetailComponent } from './org-detail/org-detail.component';
import { ReadingInsertComponent } from './reading-insert/reading-insert.component';
import { ReadingDetailsComponent } from './reading-details/reading-details.component';
import { ReadingEditComponent } from './reading-edit/reading-edit.component';
import { StudentDetailsComponent } from './student-details/student-details.component';
import { TagReadingsComponent } from './tag-readings/tag-readings.component';
import { TagStudentsComponent } from './tag-students/tag-students.component';
import { StudentListComponent } from './student-list/student-list.component';
import { TagListComponent } from './tag-list/tag-list.component';
import { WordListComponent } from './word-list/word-list.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'select' },
  {
    path: 'select',
    component: OrgDashboardComponent
  },
  {
    path: 'details/:orgId',
    component: OrgDetailComponent
  },
  {
    path: 'details/:orgId/students',
    component: StudentListComponent
  },
  {
    path: 'details/:orgId/words',
    component: WordListComponent
  },
  {
    path: 'details/:orgId/tags',
    component: TagListComponent
  },
  {
    path: 'details/:orgId/reading/insert',
    component: ReadingInsertComponent
  },
  {
    path: 'details/:orgId/reading/edit/:readingId',
    component: ReadingEditComponent
  },
  {
    path: 'details/:orgId/reading/details/:readingId',
    component: ReadingDetailsComponent
  },
  {
    path: 'details/:orgId/student/details/:studentId',
    component: StudentDetailsComponent
  },
  {
    path: 'details/:orgId/tag/:tagId/readings',
    component: TagReadingsComponent
  },
  {
    path: 'details/:orgId/tag/:tagId/students',
    component: TagStudentsComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrgDashboardRoutingModule { }

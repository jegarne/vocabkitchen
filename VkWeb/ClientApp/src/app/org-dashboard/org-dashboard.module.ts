import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';

import { SharedModule } from '../shared/shared.module';

import { OrgDashboardRoutingModule } from './org-dashboard-routing.module';
import { OrgDashboardComponent } from './org-dashboard.component';
import { ReadingDetailsComponent } from './reading-details/reading-details.component';
import { ReadingWordComponent } from './reading-details/reading-word/reading-word.component';
import { StudentDetailsComponent } from './student-details/student-details.component';
import { OrgDashboardNavComponent } from './org-dashboard-nav/org-dashboard-nav.component';
import { ReadingEditComponent } from './reading-edit/reading-edit.component';
import { ReadingInsertComponent } from './reading-insert/reading-insert.component';
import { OrgDetailComponent } from './org-detail/org-detail.component';


import { TextSelectDirective } from './reading-details/text-select.directive';
import { TagsSearchComponent } from './tags-search/tags-search.component';
import { TagStudentsComponent } from './tag-students/tag-students.component';
import { TagReadingsComponent } from './tag-readings/tag-readings.component';
import { TagListComponent } from './tag-list/tag-list.component';
import { StudentListComponent } from './student-list/student-list.component';
import { DefinitionModalComponent } from './definition-modal/definition-modal.component';
import { WordListComponent } from './word-list/word-list.component';


@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    OrgDashboardRoutingModule,
    FormsModule,
    SharedModule
  ],
  declarations: [
    OrgDashboardComponent,
    ReadingDetailsComponent,
    ReadingWordComponent,
    StudentDetailsComponent,
    OrgDashboardNavComponent,
    ReadingEditComponent,
    ReadingInsertComponent,
    OrgDetailComponent,
    TextSelectDirective,
    TagsSearchComponent,
    TagStudentsComponent,
    TagReadingsComponent,
    TagListComponent,
    StudentListComponent,
    DefinitionModalComponent,
    WordListComponent
  ],
  entryComponents: [DefinitionModalComponent]
})
export class OrgDashboardModule { }

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ReadingListComponent } from './reading-list/reading-list.component';
import { WordListComponent } from './word-list/word-list.component';
import { PretestComponent } from './pretest/pretest.component';
import { ReadComponent } from './read/read.component';
import { StudyComponent } from './study/study.component';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'words' },
  {
    path: 'readings',
    component: ReadingListComponent
  },
  {
    path: 'words',
    component: WordListComponent
  },
  {
    path: 'pretest/:readingId',
    component: PretestComponent
  },
  {
    path: 'read/:readingId',
    component: ReadComponent
  },
  {
    path: 'study/:readingId',
    component: StudyComponent
  },
  {
    path: 'study',
    component: StudyComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StudentRoutingModule { }

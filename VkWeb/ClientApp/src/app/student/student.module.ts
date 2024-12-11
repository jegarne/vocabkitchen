import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { StudentRoutingModule } from './student-routing.module';
import { WordListComponent } from './word-list/word-list.component';
import { ReadComponent } from './read/read.component';
import { StudyComponent } from './study/study.component';
import { StudentNavComponent } from './student-nav/student-nav.component';
import { ReadingListComponent } from './reading-list/reading-list.component';
import { PretestComponent } from './pretest/pretest.component';
import { FlashCardComponent } from './study/components/flash-card/flash-card.component';
import { MultipleChoiceComponent } from './study/components/multiple-choice/multiple-choice.component';
import { SpellingComponent } from './study/components/spelling/spelling.component';
import { LetterBoxesComponent } from './study/components/letter-boxes/letter-boxes.component';
import { FocusInputDirective } from './directives/focus-input.directive';
import { ClozeComponent } from './study/components/cloze/cloze.component';
import { SharedModule } from '../shared/shared.module';
import { WordExpanderComponent } from './word-list/components/word-expander/word-expander.component';
import { DefinitionModalComponent } from './definition-modal/definition-modal.component';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    StudentRoutingModule,
    FormsModule,
    SharedModule
  ],
  declarations: [
    WordListComponent,
    WordExpanderComponent,
    ReadComponent,
    StudyComponent,
    StudentNavComponent,
    ReadingListComponent,
    PretestComponent,
    FlashCardComponent,
    MultipleChoiceComponent,
    SpellingComponent,
    LetterBoxesComponent,
    FocusInputDirective,
    ClozeComponent,
    WordExpanderComponent,
    DefinitionModalComponent
  ],
  entryComponents: [DefinitionModalComponent]
})
export class StudentModule { }

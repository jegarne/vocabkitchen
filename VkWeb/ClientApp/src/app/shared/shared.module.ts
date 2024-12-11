// include directives/components commonly used in features modules in this shared modules
// and import me into the feature module
// importing them individually results in: Type xxx is part of the declarations of 2 modules: ... Please consider moving to a higher module...
// https://github.com/angular/angular/issues/10646

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatRadioModule } from '@angular/material/radio';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';

import { NgxAudioPlayerModule } from 'ngx-audio-player';

import {
  CollapseModule,
  BsDropdownModule,
  ModalModule
} from 'ngx-bootstrap';

import { myFocus } from './directives/focus.directive';
import { SpinnerComponent } from './spinner/spinner.component';
import { ControlMessages } from './control-messages.component';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { ServerErrorsComponent } from './server-errors/server-errors.component';
import { ContentInterceptor } from './interceptors/content.interceptor';
import { EmailValidator } from './directives/email.validator.directive';
import { DraggableDirective } from './directives/draggable.directive';
import { WordAudioPlayerComponent } from './word-audio-player/word-audio-player.component';

@NgModule({
  imports: [
    CommonModule,
    MatInputModule,
    MatAutocompleteModule,
    MatChipsModule,
    MatIconModule,
    MatCheckboxModule,
    MatTabsModule,
    CollapseModule.forRoot(),
    BsDropdownModule.forRoot(),
    MatButtonToggleModule,
    MatRadioModule,
    MatListModule,
    MatExpansionModule,
    ModalModule.forRoot(),
    MatTableModule,
    MatSortModule,
    NgxAudioPlayerModule
  ],
  exports: [
    myFocus,
    SpinnerComponent,
    ControlMessages,
    ServerErrorsComponent,
    MatInputModule,
    MatAutocompleteModule,
    MatChipsModule,
    MatIconModule,
    MatCheckboxModule,
    MatTabsModule,
    CollapseModule,
    BsDropdownModule,
    MatButtonToggleModule,
    MatRadioModule,
    MatListModule,
    MatExpansionModule,
    ModalModule,
    MatTableModule,
    MatSortModule,
    WordAudioPlayerComponent,
    NgxAudioPlayerModule
  ],
  declarations: [
    myFocus,
    SpinnerComponent,
    ControlMessages,
    ServerErrorsComponent,
    EmailValidator,
    DraggableDirective,
    WordAudioPlayerComponent
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ContentInterceptor,
      multi: true
    }
  ]
})
export class SharedModule { }

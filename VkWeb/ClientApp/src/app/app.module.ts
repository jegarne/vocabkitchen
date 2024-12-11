import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule } from '@angular/material/toolbar';

import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core/core.module';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './public/nav-menu.component';
import { ProfilerComponent } from './public/profiler.component';
import { SignUpComponent } from './public/sign-up.component';
import { PasswordResetComponent } from './public/password-reset.component';
import { LoginFormComponent } from './public/login-form.component';


import { AuthGuard } from './auth.guard';
import { RequestPasswordResetComponent } from './public/request-password-reset.component';
import { AcceptInviteComponent } from './public/accept-invite/accept-invite.component';
import { FeedbackComponent } from './public/feedback/feedback.component';
import { IssueLogComponent } from './public/issue-log/issue-log.component';
import { ResendConfirmationComponent } from './public/resend-confirmation/resend-confirmation.component';
import { HomeComponent } from './public/home/home.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    ProfilerComponent,
    SignUpComponent,
    PasswordResetComponent,
    LoginFormComponent,
    RequestPasswordResetComponent,
    AcceptInviteComponent,
    FeedbackComponent,
    IssueLogComponent,
    ResendConfirmationComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    SharedModule,
    ReactiveFormsModule,
    CoreModule
  ],
  providers: [
    AuthGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

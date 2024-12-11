import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ProfilerComponent } from './public/profiler.component';
import { SignUpComponent } from './public/sign-up.component';
import { LoginFormComponent } from './public/login-form.component';
import { PasswordResetComponent } from './public/password-reset.component';
import { AcceptInviteComponent } from './public/accept-invite/accept-invite.component';
import { FeedbackComponent } from './public/feedback/feedback.component';
import { IssueLogComponent } from './public/issue-log/issue-log.component';
import { ResendConfirmationComponent } from './public/resend-confirmation/resend-confirmation.component';
import { HomeComponent } from './public/home/home.component';

import { AuthGuard } from './auth.guard';

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'home' },
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'profile',
    component: ProfilerComponent
  },
  {
    path: 'sign-up',
    component: SignUpComponent
  },
  {
    path: 'login',
    component: LoginFormComponent
  },
  {
    path: 'reset-password',
    component: PasswordResetComponent
  },
  {
    path: 'request-email-confirmation',
    component: ResendConfirmationComponent
  },
  {
    path: 'invite',
    component: AcceptInviteComponent
  },
  {
    path: 'feedback',
    component: FeedbackComponent
  },
  {
    path: 'updates',
    component: IssueLogComponent
  },
  {
    path: 'org-admin',
    loadChildren: 'app/org-admin/org-admin.module#OrgAdminModule',
    canActivate: [AuthGuard]
  },
  {
    path: 'org',
    loadChildren: 'app/org-dashboard/org-dashboard.module#OrgDashboardModule',
    canActivate: [AuthGuard]
  },
  {
   path: 'student',
   loadChildren: 'app/student/student.module#StudentModule'
  },
  {
    path: 'user-profile',
    loadChildren: 'app/user-profile/user-profile.module#UserProfileModule'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

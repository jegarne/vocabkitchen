import { NgModule, Optional, SkipSelf } from '@angular/core';

import { OrgAdminService } from '@services/org-admin.service';
import { OrgService } from '@services/org.service';
import { ReadingService } from '@services/reading.service';
import { StudentService } from '@services/student.service';
import { ProfilerService } from '@services/profiler.service';
import { AuthService } from '@services/auth.service';
import { ConfigService } from '@services/config.service';
import { AccessService } from '@services/access.service';
import { ValidationService } from '@services/validation.service';
import { UserProfileService } from '@services/user-profile.service';
import { WordService } from '@services/word.service';
import { FeedbackService } from '@services/feedback.service';


@NgModule({
  providers: [
    OrgAdminService,
    OrgService,
    ReadingService,
    StudentService,
    ProfilerService,
    AuthService,
    ConfigService,
    ValidationService,
    AccessService,
    UserProfileService,
    WordService,
    FeedbackService
  ]
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() core: CoreModule) {
    if (core) {
      throw new ErrorEvent('CoreModule was already loaded.');
    }
  }
}

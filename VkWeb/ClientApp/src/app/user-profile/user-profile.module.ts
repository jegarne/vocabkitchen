import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';

import { UserProfileComponent } from './user-profile.component';
import { UserProfileRoutingModule } from './user-profile-routing.module';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    UserProfileRoutingModule,
    SharedModule
  ],
  declarations: [
    UserProfileComponent
  ]
})
export class UserProfileModule { }

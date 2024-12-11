import { Component } from '@angular/core';
import { AuthService } from '@services/auth.service';
import { Credentials } from '@models/credentials.interface';

@Component({
  selector: 'request-password-reset',
  templateUrl: './request-password-reset.component.html',
  styleUrls: ['./request-password-reset.component.css']
})
export class RequestPasswordResetComponent {
  showForm = false;
  result: string = '';
  credentials: Credentials = { email: '', password: '' };
  errors: string;

  constructor(
    private userService: AuthService,
  ) { }

  showResetForm() {
    this.showForm = true;
    return false;
  }

  requestReset() {
    this.errors = '';
    this.userService.requestPasswordReset(this.credentials.email)
      .finally(() => { })
      .subscribe(
        (result) => {
          this.result = result.value;
          this.errors = '';
        },
        errors => {
          this.errors = errors;
          this.result = '';
        });
  }
}


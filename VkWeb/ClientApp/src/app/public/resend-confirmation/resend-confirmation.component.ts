import { Component, OnInit } from '@angular/core';
import { AuthService } from '@services/auth.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'resend-confirmation',
  templateUrl: './resend-confirmation.component.html',
  styleUrls: ['./resend-confirmation.component.css']
})
export class ResendConfirmationComponent implements OnInit {

  serverErrors: string;
  resetForm: FormGroup;
  wasSuccessful = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
  ) {
    this.resetForm = this.createFormGroup();
  }

  ngOnInit() {
  }

  createFormGroup() {
    return this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  onSubmit() {
    this.wasSuccessful = false;
    this.serverErrors = '';

    const value = this.resetForm.value;
    this.authService.resendConfirmation(value.email)
      .subscribe(
        () => {
          this.wasSuccessful = true;
        },
        errors => {
          this.wasSuccessful = false;
          this.serverErrors = errors;
        });

  }
}

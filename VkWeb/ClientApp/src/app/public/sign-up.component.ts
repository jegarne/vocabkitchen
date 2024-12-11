import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '@services/auth.service';

@Component({
  selector: 'sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent {

  serverErrors: string;
  isRequesting: boolean;
  signUpForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: AuthService,
    private router: Router
  ) {
    this.signUpForm = this.createFormGroup();
  }

  createFormGroup() {
    return this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.compose([
        Validators.required,
        Validators.minLength(4)
      ])],
      confirmPassword: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      organizationName: ['', Validators.required]
    }, {
      validators: this.password.bind(this)
    });
  }

  password(formGroup: FormGroup) {
    const { value: password } = formGroup.get('password');
    const { value: confirmPassword } = formGroup.get('confirmPassword');
    return password === confirmPassword ? null : { passwordMismatch: true };
  }

  onSubmit() {
    this.isRequesting = true;
    this.serverErrors = '';

    const value = this.signUpForm.value;
    this.userService.register(value.email, value.password, value.firstName, value.lastName, value.organizationName)
      .finally(() => this.isRequesting = false)
      .subscribe(
        (result) => {
          this.router.navigate(['/login'], { queryParams: { brandNew: true, email: result.email } });
        },
        errors => {
          this.serverErrors = errors;
        });

  }
}

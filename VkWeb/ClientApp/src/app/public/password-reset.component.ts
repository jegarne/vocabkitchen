import { Component, OnInit } from '@angular/core';
import { AuthService } from '@services/auth.service';
import { Credentials } from './models/credentials.interface';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'password-reset',
  templateUrl: './password-reset.component.html',
  styleUrls: ['./password-reset.component.css']
})
export class PasswordResetComponent {
  result: string = '';
  noTokenOrId = false;
  serverErrors: string;
  userId: string;
  token: string;

  signUpForm: FormGroup;

  constructor(
    private userService: AuthService,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute
  ) {
    this.signUpForm = this.createFormGroup();
  }

  ngOnInit() {

    this.activatedRoute.queryParams.subscribe(
      (param: any) => {
        this.userId = param['userId'];
        this.token = param['token'];
      });

    this.noTokenOrId = !this.userId || !this.token;
  }

  createFormGroup() {
    return this.fb.group({
      password: ['', Validators.compose([
        Validators.required,
        Validators.minLength(4)
      ])],
      confirmPassword: ['', Validators.required],
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
    this.serverErrors = '';
    let newPassword = this.signUpForm.get('password').value;
    this.userService.resetPassword(this.userId, this.token, newPassword)
      .finally(() => { })
      .subscribe(
        (result) => {
          this.result = result.value;
        },
        errors => {
          this.serverErrors = errors;
        });
  }
}

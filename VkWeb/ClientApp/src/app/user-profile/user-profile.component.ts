import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { UserProfileService } from '@services/user-profile.service';
import { User } from '@models/user';


@Component({
  selector: 'user-profile',
  templateUrl: './user-profile.component.html'
})
export class UserProfileComponent implements OnInit {

  serverErrors: string;
  result: string = '';
  user: User = new User();

  public loginForm: FormGroup;

  errorMessages = {
    'oldPassword': [
      { type: 'required', message: 'Required' }
    ],
    'newPassword': [
      { type: 'required', message: 'Required' },
      { type: 'minlength', message: 'Minimum length 4' }
    ]
  }

  constructor(
    private fb: FormBuilder,
    private userService: UserProfileService
  ) {
    this.loginForm = this.fb.group({
      oldPassword: new FormControl('', Validators.compose([
        Validators.required
      ])),
      newPassword: new FormControl('', Validators.compose([
        Validators.required,
        Validators.minLength(4)
      ])),
      confirmPassword: new FormControl('', Validators.compose([
        Validators.required
      ])),
    }, {
      validators: this.password.bind(this)
    });
  }

  ngOnInit() {
    this.userService.getUser().subscribe(user => this.user = user);
  }

  password(formGroup: FormGroup) {
    const { value: password } = formGroup.get('newPassword');
    const { value: confirmPassword } = formGroup.get('confirmPassword');
    return password === confirmPassword ? null : { passwordMismatch: true };
  }

  submitNewPassword() {
    if (this.loginForm.valid) {
      let value = this.loginForm.value;
      this.userService.changePassword(value.oldPassword, value.newPassword)
        .subscribe(
          (result) => {
            this.loginForm.reset();
            this.result = result.value;
            this.serverErrors = '';
          },
          errors => {
            this.serverErrors = errors;
            this.result = '';
          });
    }
  }


}

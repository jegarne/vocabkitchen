import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '@services/auth.service';

@Component({
  selector: 'accept-invite',
  templateUrl: './accept-invite.component.html',
  styleUrls: ['./accept-invite.component.css']
})
export class AcceptInviteComponent implements OnInit, OnDestroy {

  private sub: any;

  serverErrors: string;
  isRequesting: boolean;
  signUpForm: FormGroup;
  orgId: string;
  inviteType: string;
  inviteEmail: string;
  invalidInviteUrl = false;

  constructor(
    private fb: FormBuilder,
    private userService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
  ) {
    this.signUpForm = this.createFormGroup();
  }

  ngOnInit() {
    //this.orgId = this.route.snapshot.queryParamMap.get('orgId');
    //this.inviteType = this.route.snapshot.queryParamMap.get('inviteType');
    this.inviteEmail = this.route.snapshot.queryParamMap.get('email');

    //if (!this.orgId || !this.inviteType || !this.inviteEmail)
    //  this.invalidInviteUrl = true;
    //else
    //  this.invalidInviteUrl = false;

    this.signUpForm.get('email').setValue(this.inviteEmail);
    this.signUpForm.get('email').disable();
  }

  ngOnDestroy() {
    if (this.sub)
      this.sub.unsubscribe();
  }

  createFormGroup() {
    return this.fb.group({
      email: '',
      password: ['', Validators.compose([
        Validators.required,
        Validators.minLength(4)
      ])],
      confirmPassword: ['', Validators.required],
      firstName: '',
      lastName: '',
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
    this.sub = this.userService.acceptInvite(this.inviteEmail, value.password, value.firstName, value.lastName)
      .finally(() => this.isRequesting = false)
      .subscribe(
        () => {
          this.router.navigate(['/login'], { queryParams: { brandNew: true, email: value.email, isConfirmed: true } });
        },
        errors => {
          this.serverErrors = errors;
        });
  }
}

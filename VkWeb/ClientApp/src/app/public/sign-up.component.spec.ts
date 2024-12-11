import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { SignUpComponent } from './sign-up.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '@services/auth.service';
import { Router } from '@angular/router';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { Subject } from 'rxjs';
import { UserRegistration } from './models/user.registration.interface';

class MockUserService {
  constructor() {
    this.subject = new Subject();
  }
  subject;
  register() {
    return this.subject;
  }
}

class MockRouter {
  navigate(routes) {
    return null;
  }
}

describe('SignUpComponent', () => {
  let component: SignUpComponent;
  let fixture: ComponentFixture<SignUpComponent>;
  let mockUserServiceRef;
  let mockRouterRef;

  beforeEach(async(() => {

    TestBed.configureTestingModule({
      imports: [
        FormsModule,
        ReactiveFormsModule
      ],
      declarations: [
        SignUpComponent
      ],
      providers: [
        { provide: AuthService, useClass: MockUserService },
        { provide: Router, useClass: MockRouter },
      ],
      schemas: [NO_ERRORS_SCHEMA]
    });

    fixture = TestBed.createComponent(SignUpComponent);
    component = fixture.componentInstance;
    mockUserServiceRef = TestBed.get(AuthService);
    mockRouterRef = TestBed.get(Router);

    fixture.detectChanges();

  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('registerUser', () => {

    it('should pass form values to user service', () => {
      const value = new UserRegistration();
      value.email = 'em';
      value.firstName = 'fn';
      value.lastName = 'ln';
      value.organizationName = 'here';
      value.password = 'pw';
      component.signUpForm.controls['email'].setValue(value.email);
      component.signUpForm.controls['password'].setValue(value.password);
      component.signUpForm.controls['firstName'].setValue(value.firstName);
      component.signUpForm.controls['lastName'].setValue(value.lastName);
      component.signUpForm.controls['organizationName'].setValue(value.organizationName);


      spyOn(mockUserServiceRef, 'register').and.callThrough();
      component.onSubmit();
      expect(mockUserServiceRef.register)
        .toHaveBeenCalledWith(
          value.email,
          value.password,
          value.firstName,
          value.lastName,
          value.organizationName
        );
    });

    it('should route to org-admin on success', () => {
      const value = new UserRegistration();
      value.email = 'em';
      component.signUpForm.controls['email'].setValue(value.email);

      spyOn(mockRouterRef, 'navigate').and.callThrough();
      component.onSubmit();
      mockUserServiceRef.register().next('');
      expect(mockRouterRef.navigate)
        .toHaveBeenCalledWith(
          ['/login'],
          { queryParams: { brandNew: true, email: value.email } }
        );
    });

    it('should capture user service error', () => {
      spyOn(mockRouterRef, 'navigate').and.callThrough();
      component.onSubmit();
      mockUserServiceRef.register().error('oops');
      expect(component.serverErrors).toEqual('oops');
    });

  });


});

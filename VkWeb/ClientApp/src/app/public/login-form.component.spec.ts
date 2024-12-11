import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginFormComponent } from './login-form.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '@services/auth.service';
import { Router, ActivatedRoute } from '@angular/router';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { Subject } from 'rxjs';

class MockActivatedRoute {
    queryParams = new Subject();
}

class MockUserService {
    constructor() {
        this.subject = new Subject();
    }
    subject;
    login() {
        return this.subject;
    }
}

class MockRouter {
    navigate(routes) {
        return null;
    }
}

describe('LoginFormComponent', () => {
    let component: LoginFormComponent;
    let fixture: ComponentFixture<LoginFormComponent>;
    let mockActivatedRouteRef;
    let mockUserServiceRef;
    let mockRouterRef;

    beforeEach(async(() => {

        TestBed.configureTestingModule({
            imports: [
                FormsModule,
                ReactiveFormsModule
            ],
            declarations: [
                LoginFormComponent
            ],
            providers: [
                { provide: AuthService, useClass: MockUserService },
                { provide: Router, useClass: MockRouter },
                { provide: ActivatedRoute, useClass: MockActivatedRoute },
            ],
            schemas: [NO_ERRORS_SCHEMA]
        });

        fixture = TestBed.createComponent(LoginFormComponent);
        component = fixture.componentInstance;
        mockActivatedRouteRef = TestBed.get(ActivatedRoute);
      mockUserServiceRef = TestBed.get(AuthService);
      mockRouterRef = TestBed.get(Router);

        fixture.detectChanges();

    }));

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    describe('ngOnInit', () => {

        it('should set brandNew from activatedRoute', () => {
            mockActivatedRouteRef.queryParams.next({ brandNew: true });
            expect(component.brandNew).toEqual(true);
        });

        it('should set email from activatedRoute', () => {
            const obj = { email: 'foo@bar.com' };
            mockActivatedRouteRef.queryParams.next(obj);
            expect(component.credentials.email).toEqual(obj.email);
        });

    });

    describe('login', () => {

        it('should call user service if valid', () => {
            const value = {email: 'em', password: 'pw'};
            const valid = true;
            spyOn(mockUserServiceRef, 'login').and.callThrough();
            component.login({value, valid});
            expect(mockUserServiceRef.login).toHaveBeenCalledWith(value.email, value.password);
        });

        it('should not call user service if not valid', () => {
            const value = {email: 'em', password: 'pw'};
            const valid = false;
            spyOn(mockUserServiceRef, 'login').and.callThrough();
            component.login({value, valid});
            expect(mockUserServiceRef.login).toHaveBeenCalledTimes(0);
        });

        it('should route to org-admin on success', () => {
            const value = {email: 'em', password: 'pw'};
            const valid = true;
            spyOn(mockRouterRef, 'navigate').and.callThrough();
            component.login({value, valid});
            mockUserServiceRef.login().next('');
            expect(mockRouterRef.navigate).toHaveBeenCalledWith(['/org-admin']);
        });

        it('should capture user service error', () => {
            const value = {email: 'em', password: 'pw'};
            const valid = true;
            spyOn(mockRouterRef, 'navigate').and.callThrough();
            component.login({value, valid});
            mockUserServiceRef.login().error('oops');
            expect(component.errors).toEqual('oops');
        });

    });


});

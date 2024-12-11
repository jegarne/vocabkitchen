import { async, inject, TestBed } from '@angular/core/testing';
import { AuthService } from './auth.service';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ConfigService } from '@services/config.service';

class MockConfigService {
  getApiURI() {
    return 'foo.com';
  }
}

describe('UserService', () => {
  let httpMock: HttpTestingController;

  beforeEach(async(() => {

    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      declarations: [

      ],
      providers: [
        AuthService,
        { provide: ConfigService, useClass: MockConfigService }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    });

    httpMock = TestBed.get(HttpTestingController);

  }));

  afterEach(() => {
    httpMock.verify();
  });

  it('should create', inject([AuthService], (userService) => {
    expect(userService).toBeTruthy();
  }));

  describe('constructor when logged out', () => {
    beforeEach(async(() => {
      spyOn(localStorage, 'getItem').and.callFake((key) => {
        return 'token';
      });
    }));

    it('should set baseUrl', inject([AuthService], (userService) => {
      expect(userService.baseUrl).toEqual('foo.com');
    }));

    it('should get auth_token from local storage', inject([AuthService], (userService) => {
      expect(localStorage.getItem).toHaveBeenCalledWith('auth_token');
    }));

    it('should set isLoggedIn() true if token is in local storage', inject([AuthService], (userService) => {
      expect(userService.isLoggedIn()).toEqual(true);
    }));

    it('should emit authNavStatus of true', inject([AuthService], (userService) => {
      userService.authNavStatus$.subscribe((value) => {
        expect(value).toEqual(true);
      })
    }));
  });

  describe('constructor when logged in', () => {
    beforeEach(async(() => {
      spyOn(localStorage, 'getItem').and.callFake((key) => {
        return null;
      });
    }));

    it('should set isLoggedIn() false if token is not in local storage', inject([AuthService], (userService) => {
      expect(userService.isLoggedIn()).toEqual(false);
    }));

    it('should emit authNavStatus of false', inject([AuthService], (userService) => {
      userService.authNavStatus$.subscribe((value) => {
        expect(value).toEqual(false);
      })
    }));
  });

  describe('register', () => {

    it('should post sign up info', inject([AuthService], (userService) => {

      const dummyUserRegistration = {
        email: 'email',
        password: 'pw',
        firstName: 'fn',
        lastName: 'ln',
        location: 'here'
      };

      userService.register('email', 'pw', 'fn', 'ln', 'org').subscribe(user => {
        expect(user).toEqual(dummyUserRegistration);
      });

      const req = httpMock.expectOne(`foo.com/org`);
      expect(req.request.method).toBe('POST');
      req.flush(dummyUserRegistration);
    }));

  });

  describe('login', () => {

    beforeEach(async(() => {
      spyOn(localStorage, 'setItem').and.callFake((key, value) => {
        return null;
      });
    }));

    it('should post sign in info', inject([AuthService], (userService) => {

      const dummyResponse = {
        auth_token: 'token'
      };

      userService.login('un', 'pw').subscribe(response => {
        expect(response).toEqual(dummyResponse);
      });;

      const req = httpMock.expectOne(`foo.com/auth/login`);
      expect(req.request.method).toBe('POST');
      req.flush(dummyResponse);
    }));

    it('should set auth_token', inject([AuthService], (userService) => {

      const dummyResponse = {
        auth_token: 'token'
      };

      userService.login('un', 'pw').subscribe(response => {
        expect(localStorage.setItem).toHaveBeenCalledWith('auth_token', 'token');
      });

      const req = httpMock.expectOne(`foo.com/auth/login`);
      req.flush(dummyResponse);
    }));

    it('should cause isLoggedIn() to return true', inject([AuthService], (userService) => {

      const dummyResponse = {
        auth_token: 'token'
      };

      userService.login('un', 'pw').subscribe(response => {
        expect(userService.isLoggedIn()).toEqual(true);
      });

      const req = httpMock.expectOne(`foo.com/auth/login`);
      req.flush(dummyResponse);
    }));

    it('should cause authNavStatus$ to emit true', inject([AuthService], (userService) => {

      const dummyResponse = {
        auth_token: 'token'
      };

      userService.login('un', 'pw').subscribe(response => {
        userService.authNavStatus$.subscribe((value) => {
          expect(value).toEqual(true);
        })
      });

      const req = httpMock.expectOne(`foo.com/auth/login`);
      req.flush(dummyResponse);
    }));


  });

  describe('logout', () => {

    beforeEach(async(() => {
      spyOn(localStorage, 'removeItem').and.callFake((key) => {
        return null;
      });
    }));

    it('should delete auth_token', inject([AuthService], (userService) => {
      userService.logout();
      expect(localStorage.removeItem).toHaveBeenCalledWith('auth_token');
    }));

    it('should cause isLoggedIn() to return false', inject([AuthService], (userService) => {
      userService.logout();
      expect(userService.isLoggedIn()).toEqual(false);
    }));

    it('should cause authNavStatus$ to emit false', inject([AuthService], (userService) => {
      userService.logout();

      userService.authNavStatus$.subscribe((value) => {
        expect(value).toEqual(false);
      });

    }));


  });


});

import { async, inject, TestBed } from '@angular/core/testing';
import { ProfilerService } from './profiler.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ProfilerRequest } from '@models/profiler-request';

describe('ProfilerService', () => {
  let httpMock: HttpTestingController;

  beforeEach(async(() => {

    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      declarations: [

      ],
      providers: [
        ProfilerService,
        { provide: 'BASE_URL', useValue: 'foo.com/' }
      ]
    });

    httpMock = TestBed.get(HttpTestingController);

  }));

  afterEach(() => {
    httpMock.verify();
  });

  it('should create', inject([ProfilerService], (profileService) => {
    expect(profileService).toBeTruthy();
  }));

  describe('constructor', () => {

    it('should set baseUrl', inject([ProfilerService], (profileService) => {
      expect(profileService.baseUrl).toEqual('foo.com/');
    }));

  });

  describe('profile', () => {

    it('should make request', inject([ProfilerService], (profileService) => {

      const dummyRequest = new ProfilerRequest('cefr', 'input');
      const dummyResponse = new ProfilerRequest('cefr', 'input');
      dummyResponse.totalWordCount = 1;

      profileService.profile(dummyRequest).subscribe(response => {
        expect(response).toEqual(dummyResponse);
      });

      const req = httpMock.expectOne(`foo.com/api/profiler/profile/`);
      expect(req.request.method).toBe('POST');
      req.flush(dummyResponse);
    }));

  });

});

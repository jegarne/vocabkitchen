import { async, ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { ProfilerComponent } from './profiler.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProfilerService } from '@services/profiler.service';
import { Subject } from 'rxjs';
import { ProfilerRequest } from './models/profiler-request';
import { By } from '@angular/platform-browser';

class MockProfilerService {
    constructor() {
        this.subject = new Subject();
    }
    subject;
    profile(val1) {
        return this.subject;
    }
}

describe('ProfilerComponent', () => {
    let component: ProfilerComponent;
    let fixture: ComponentFixture<ProfilerComponent>;
    let mockProfilerServiceRef;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [
                FormsModule,
                ReactiveFormsModule
            ],
            declarations: [ProfilerComponent],
            providers: [
                { provide: ProfilerService, useClass: MockProfilerService }
            ],
            schemas: [NO_ERRORS_SCHEMA]
        }).compileComponents();

        fixture = TestBed.createComponent(ProfilerComponent);
        component = fixture.componentInstance;
        mockProfilerServiceRef = TestBed.get(ProfilerService);

        fixture.detectChanges();
    }));

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    describe('submit functions', () => {
        it('should pass correct cefr values to profilerService ', () => {
            spyOn(mockProfilerServiceRef, 'profile').and.callThrough();

            component.profilerInput = 'test';
            component.submitCefr();

            expect(mockProfilerServiceRef.profile.calls.mostRecent().args[0].profilerType).toEqual('cefr');
            expect(mockProfilerServiceRef.profile.calls.mostRecent().args[0].inputText).toEqual('test');
        });

        it('should pass correct awl values to profilerService ', () => {
            spyOn(mockProfilerServiceRef, 'profile').and.callThrough();

            component.profilerInput = 'test';
            component.submitAwl();

            expect(mockProfilerServiceRef.profile.calls.mostRecent().args[0].profilerType).toEqual('awl');
            expect(mockProfilerServiceRef.profile.calls.mostRecent().args[0].inputText).toEqual('test');
        });

        it('should set profilerService result correctly', () => {
            spyOn(mockProfilerServiceRef, 'profile').and.callThrough();
          const pr = new ProfilerRequest('', '');
            pr.totalWordCount = 1;

            component.profilerInput = 'test';
            component.submitAwl();
            mockProfilerServiceRef.profile().next(pr);

            expect(component.profileResult).toEqual(pr);
        });
    });

    describe('html template', () => {

        it('CEFR button should call submitCefr()', () => {
            spyOn(component, 'submitCefr').and.callThrough();

            const fakeEvent = { preventDefault: () => console.log('preventDefault') };
            fixture.debugElement.query(By.css('form')).triggerEventHandler('submit', fakeEvent);
            fixture.detectChanges();

            expect(component.submitCefr).toHaveBeenCalled();
        });

        it('AWL button should call submitAwl()', () => {
            spyOn(component, 'submitAwl').and.callThrough();

            const fakeEvent = { preventDefault: () => console.log('preventDefault') };
            fixture.debugElement.query(By.css('.btn-awl')).triggerEventHandler('click', fakeEvent);
            fixture.detectChanges();

            expect(component.submitAwl).toHaveBeenCalled();
        });

    });

});

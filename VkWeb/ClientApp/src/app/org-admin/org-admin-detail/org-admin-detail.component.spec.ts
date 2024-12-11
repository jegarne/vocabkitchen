import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrgAdminDetailComponent } from './org-admin-detail.component';

describe('OrgAdminDetailComponent', () => {
  let component: OrgAdminDetailComponent;
  let fixture: ComponentFixture<OrgAdminDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrgAdminDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrgAdminDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

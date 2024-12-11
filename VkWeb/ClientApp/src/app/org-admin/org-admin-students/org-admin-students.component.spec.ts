import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrgAdminStudentsComponent } from './org-admin-students.component';

describe('OrgAdminStudentsComponent', () => {
  let component: OrgAdminStudentsComponent;
  let fixture: ComponentFixture<OrgAdminStudentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrgAdminStudentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrgAdminStudentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

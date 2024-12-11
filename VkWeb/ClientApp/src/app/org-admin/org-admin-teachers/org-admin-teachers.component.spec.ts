import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrgAdminTeachersComponent } from './org-admin-teachers.component';

describe('OrgAdminTeachersComponent', () => {
  let component: OrgAdminTeachersComponent;
  let fixture: ComponentFixture<OrgAdminTeachersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrgAdminTeachersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrgAdminTeachersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

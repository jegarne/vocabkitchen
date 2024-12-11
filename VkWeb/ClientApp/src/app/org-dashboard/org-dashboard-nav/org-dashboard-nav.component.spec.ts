import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrgDashboardNavComponent } from './org-dashboard-nav.component';

describe('OrgDashboardNavComponent', () => {
  let component: OrgDashboardNavComponent;
  let fixture: ComponentFixture<OrgDashboardNavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrgDashboardNavComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrgDashboardNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

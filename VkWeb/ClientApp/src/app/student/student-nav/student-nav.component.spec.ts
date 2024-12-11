import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentNavComponent } from './student-nav.component';

describe('StudentNavComponent', () => {
  let component: StudentNavComponent;
  let fixture: ComponentFixture<StudentNavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StudentNavComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StudentNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

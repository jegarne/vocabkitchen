import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TagStudentsComponent } from './tag-students.component';

describe('TagStudentsComponent', () => {
  let component: TagStudentsComponent;
  let fixture: ComponentFixture<TagStudentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TagStudentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TagStudentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReadingEditComponent } from './reading-edit.component';

describe('ReadingEditComponent', () => {
  let component: ReadingEditComponent;
  let fixture: ComponentFixture<ReadingEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReadingEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReadingEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

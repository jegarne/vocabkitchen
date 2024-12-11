import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReadingInsertComponent } from './reading-insert.component';

describe('ReadingInsertComponent', () => {
  let component: ReadingInsertComponent;
  let fixture: ComponentFixture<ReadingInsertComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReadingInsertComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReadingInsertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

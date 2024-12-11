import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReadingDetailsComponent } from './reading-details.component';

describe('ReadingDetailsComponent', () => {
  let component: ReadingDetailsComponent;
  let fixture: ComponentFixture<ReadingDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReadingDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReadingDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

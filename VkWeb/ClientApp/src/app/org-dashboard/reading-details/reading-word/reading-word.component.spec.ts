import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReadingWordComponent } from './reading-word.component';

describe('ReadingWordComponent', () => {
  let component: ReadingWordComponent;
  let fixture: ComponentFixture<ReadingWordComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReadingWordComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReadingWordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LetterBoxesComponent } from './letter-boxes.component';

describe('LetterBoxesComponent', () => {
  let component: LetterBoxesComponent;
  let fixture: ComponentFixture<LetterBoxesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LetterBoxesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LetterBoxesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

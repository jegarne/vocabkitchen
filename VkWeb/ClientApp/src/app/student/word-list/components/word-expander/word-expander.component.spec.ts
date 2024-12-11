import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WordExpanderComponent } from './word-expander.component';

describe('WordExpanderComponent', () => {
  let component: WordExpanderComponent;
  let fixture: ComponentFixture<WordExpanderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WordExpanderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WordExpanderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SpellingComponent } from './spelling.component';

describe('SpellingComponent', () => {
  let component: SpellingComponent;
  let fixture: ComponentFixture<SpellingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpellingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpellingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

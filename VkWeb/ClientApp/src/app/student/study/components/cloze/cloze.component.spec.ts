import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClozeComponent } from './cloze.component';

describe('ClozeComponent', () => {
  let component: ClozeComponent;
  let fixture: ComponentFixture<ClozeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClozeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClozeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

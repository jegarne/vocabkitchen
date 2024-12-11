import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PretestComponent } from './pretest.component';

describe('PretestComponent', () => {
  let component: PretestComponent;
  let fixture: ComponentFixture<PretestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PretestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PretestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

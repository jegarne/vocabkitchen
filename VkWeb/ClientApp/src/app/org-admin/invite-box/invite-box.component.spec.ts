import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InviteBoxComponent } from './invite-box.component';

describe('InviteBoxComponent', () => {
  let component: InviteBoxComponent;
  let fixture: ComponentFixture<InviteBoxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InviteBoxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InviteBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

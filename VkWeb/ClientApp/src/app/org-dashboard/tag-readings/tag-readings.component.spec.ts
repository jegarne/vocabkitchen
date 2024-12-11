import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TagReadingsComponent } from './tag-readings.component';

describe('TagReadingsComponent', () => {
  let component: TagReadingsComponent;
  let fixture: ComponentFixture<TagReadingsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TagReadingsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TagReadingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

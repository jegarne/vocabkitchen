import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WordAudioPlayerComponent } from './word-audio-player.component';

describe('WordAudioPlayerComponent', () => {
  let component: WordAudioPlayerComponent;
  let fixture: ComponentFixture<WordAudioPlayerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WordAudioPlayerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WordAudioPlayerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

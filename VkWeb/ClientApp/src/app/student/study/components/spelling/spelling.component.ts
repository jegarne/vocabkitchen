import { Component, OnInit, EventEmitter, Output, Input, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  selector: 'spelling',
  templateUrl: './spelling.component.html',
  styleUrls: ['./spelling.component.css']
})
export class SpellingComponent implements OnInit, OnChanges {
  @Input() word: string;
  @Input() allowSound = true;
  @Output() isCorrect: EventEmitter<boolean> = new EventEmitter<boolean>();
  fileName: string;

  constructor() { }

  ngOnInit() {

  }

  ngOnChanges(changes: SimpleChanges) {
    this.word = changes.word.currentValue;
  }

  handleAnswer(isCorrect) {
    this.isCorrect.emit(isCorrect);
  }

}

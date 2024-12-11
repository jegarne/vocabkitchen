import { Component, OnInit, Input, Output, EventEmitter, SimpleChanges, OnChanges, HostListener, Inject, ViewChild } from '@angular/core';
import { DOCUMENT } from '@angular/common';


@Component({
  selector: 'letter-boxes',
  templateUrl: './letter-boxes.component.html',
  styleUrls: ['./letter-boxes.component.css']
})
export class LetterBoxesComponent implements OnInit, OnChanges {
  @Input() word: string;
  @Output() isCorrect: EventEmitter<boolean> = new EventEmitter<boolean>();

  screenWidth: any;
  scrollPosition = 0;
  @ViewChild('spellingBoxes', { static: false }) spellingBoxes; 

  correctAnswer = new Array<string>();
  userAnswer = new Array<string>();
  disabledIndexes = [0];
  skippedIndexes = new Array<number>();
  focusedInput = 1;

  constructor(
    @Inject(DOCUMENT) private document: Document
  ) {
    this.onResize();
  }

  ngOnInit() {
    this.init();
    this.scrollPosition = this.document.body.scrollTop;
  }

  ngOnChanges(changes: SimpleChanges) {
    this.word = changes.word.currentValue;
    this.init();
  }

  @HostListener('window:resize', ['$event'])
  onResize(event?) {
    this.screenWidth = window.innerWidth;
  }

  handleScroll(e) {
    window.scrollTo(this.scrollPosition, 0);
    this.document.body.scrollTop = this.scrollPosition;
  }

  isVowel = (char) => ['a', 'e', 'i', 'o', 'u'].indexOf(char) !== -1;
  isSpace = (char) => char === ' ';
  isPrefilled = (char, index) => {
    return this.disabledIndexes.indexOf(index) !== -1;
  };
  hasFocus = (index) => {
    if (index === this.focusedInput) {
      return true;
    }
    return false;
  }

  init() {
    if (!this.word) return;

    this.disabledIndexes = [0];
    this.skippedIndexes = new Array<number>();
    this.correctAnswer = this.word.split('');
    this.focusedInput = 1;
    this.userAnswer = this.correctAnswer.map((l, i) => {
      // prefill first letter
      if (i === 0) return l;
      // prefill first letter of subsequent words
      if (this.correctAnswer[i - 1] === ' ') {
        this.disabledIndexes.push(i);
        return l;
      }
      // prefill empty spaces
      if (this.correctAnswer[i] === ' ') {
        this.skippedIndexes.push(i);
        return ' ';
      }
      return '';
    });
  }

  handleBackspace = (event: KeyboardEvent, index) => {
    if (event.key === "Backspace" && event.code !== 'focusInput') {
      this.userAnswer[index] = '';
      this.focusPreviousInput(index);
      return;
    }

    // hack to keep focus on input in safari
    if (event.key === "Backspace") {
      this.userAnswer[index] = '';
      return;
    }
  }

  checkAnswer = (event: KeyboardEvent, index) => {
    const target = event.target as HTMLInputElement;
    if (!target.value) return;
    this.userAnswer[index] = target.value;

    for (var i = this.correctAnswer.length; i--;) {
      if (this.correctAnswer[i] !== this.userAnswer[i]) {
        this.focusNextInput(index);
        return;
      }
    }

    this.isCorrect.emit(true);
  }

  private focusNextInput(index) {
    if (this.skippedIndexes.indexOf(index + 1) !== -1) {
      this.focusedInput = index + 3;
      return;
    }
    this.focusedInput = index + 1;
  }

  private focusPreviousInput(index) {
    if (this.focusedInput === 1) {
      //let event = new KeyboardEvent('keydown', { 'bubbles': true, 'key': 'Backspace' });
      //this.hostElement.nativeElement.dispatchEvent(event);
      return
    };

    if (this.skippedIndexes.indexOf(index - 2) !== -1) {
      this.focusedInput = index - 3;
      return;
    }

    this.focusedInput = index - 1;
  }

  private isAlphaNumeric = (event: KeyboardEvent) => {
    if (event.keyCode >= 48 && event.keyCode <= 57) {
      // Number
      return true;
    } else if (event.keyCode >= 65 && event.keyCode <= 90) {
      // Alphabet upper case
      return true;
    } else if (event.keyCode >= 97 && event.keyCode <= 122) {
      // Alphabet lower case
      return true;
    } else if (event.keyCode == 229) {
      // weird android bug
      return true;
    }
    return false;
  };
}

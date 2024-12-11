import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { BehaviorSubject, Subscription, zip, Observable } from 'rxjs';
import { StudentWord } from '@models/student-word';

@Component({
  selector: 'multiple-choice',
  templateUrl: './multiple-choice.component.html',
  styleUrls: ['./multiple-choice.component.css']
})
export class MultipleChoiceComponent implements OnInit {

  @Input()
  distractors$: Observable<string[]>;

  private _word = new BehaviorSubject<StudentWord>(new StudentWord('','','','',[]));

  @Input()
  set word(value) {
    this._word.next(value);
  };

  get word() {
    return this._word.getValue();
  }

  @Output() isCorrect: EventEmitter<boolean> = new EventEmitter<boolean>();

  options: string[] = [];

  sub: Subscription = new Subscription();

  constructor() { }

  ngOnInit() {
    const data = zip(this.distractors$, this._word);
    this.sub.add(data.subscribe(([distractors, word]) => {
      if (!distractors || !word) return;
      this.options = this.selectDistractors(distractors, word.word, 4);
      this.options.push(word.word);
      this.shuffle(this.options);
    }));

  }

  selectDistractors = (wordsArray, wordToExclude, howManyYouNeed) => {
    var randWords = [];
    var arrayCopy = wordsArray.slice(0); // copy the array so the original isn't affected
    for (var i = 0; randWords.length < howManyYouNeed; i++) {
      var randIndex = Math.floor(Math.random() * arrayCopy.length);
      var wordToAdd = arrayCopy.splice(randIndex, 1);
      if (wordToAdd[0].word != wordToExclude) {
        randWords.push(wordToAdd[0]);
      }
    }
    return randWords;
  };

  shuffle = (array) => {

    let currentIndex = array.length;
    let temporaryValue, randomIndex;

    while (0 !== currentIndex) {
      randomIndex = Math.floor(Math.random() * currentIndex);
      currentIndex -= 1;

      temporaryValue = array[currentIndex];
      array[currentIndex] = array[randomIndex];
      array[randomIndex] = temporaryValue;
    }

    return array;

  };

  checkAnswer = (selectedWord: string) => {
    if (selectedWord === this.word.word) {
      this.isCorrect.emit(true);
    } else {
      this.isCorrect.emit(false);
    }
  }

}

import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { StudentService } from '@services/student.service';
import { StudentWord } from '@models/student-word';
import { Location } from '@angular/common';
import { Observable } from 'rxjs';

@Component({
  selector: 'study',
  templateUrl: './study.component.html',
  styleUrls: ['./study.component.css']
})
export class StudyComponent implements OnInit, OnDestroy {
  readingId = '';
  private sub: any;
  words: StudentWord[] = new Array<StudentWord>();

  currentWord: StudentWord;
  currentWordIndex = 0;
  step = 0;

  distractors$: Observable<string[]>;

  constructor(
    private studentService: StudentService,
    private route: ActivatedRoute,
    private location: Location,

  ) { }

  ngOnInit() {
    this.distractors$ = this.studentService.getRandomWords();
    this.sub = this.route.params.subscribe(params => {
      this.readingId = params['readingId'] ? params['readingId'] : '';
      this.studentService.getUnknownWords(this.readingId).subscribe(
        result => {
          this.words = this.shuffle(result);
          if (this.words.length)
            this.setCurrentWord();
          else
            this.location.back();
        });
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  setCurrentWord() {
    if (this.words && this.words.length)
      this.currentWord = this.words[this.currentWordIndex];
  }

  nextStep(logFailure: boolean, failureType: string = '') {

    if (logFailure) {
      this.addFailedAttempt(failureType);
    }

    if (this.step === 2) {
      this.step = 0;
      this.nextWord();
    } else {
      this.step++;
    }
  }

  nextWord() {
    if (this.words.length === this.currentWordIndex + 1) {
      this.exit();
      return;
    }

    this.currentWordIndex++
    this.currentWord = this.words[this.currentWordIndex];
  }

  handleMultipleChoiceAnswer(isCorrect, index) {
    if (isCorrect) {
      this.sub.add(this.studentService
        .addWordAttempt(
          this.currentWord,
          'meaning',
          isCorrect
        )
        .subscribe(result => {
          this.nextStep(false);
        }));
    } else {
      this.nextStep(true, 'meaning');
    }
  }

  handleClozeAnswer(isCorrect, index) {
    if (isCorrect) {
      this.sub.add(this.studentService
        .addWordAttempt(
          this.currentWord,
          'cloze',
          true
        )
        .subscribe(result => {
          this.nextStep(false);
        }));
    }
  }

  handleSpellingAnswer(isCorrect, index) {
    if (isCorrect) {
      this.sub.add(this.studentService
        .addWordAttempt(
          this.currentWord,
          'spelling',
          true
        )
        .subscribe(result => {
          this.nextStep(false);
        }));
    }
  }

  addFailedAttempt(type: string) {
    this.sub.add(this.studentService
      .addWordAttempt(
        this.currentWord,
        type,
        false
      ).subscribe(result => {
        console.log(result);
      }));
  }

  exit() {
    this.location.back();
  }

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

}

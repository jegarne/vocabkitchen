import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { Location } from '@angular/common';
import { StudentWord } from '@models/student-word';
import { StudentService } from '@services/student.service';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'pretest',
  templateUrl: './pretest.component.html',
  styleUrls: ['./pretest.component.css']
})
export class PretestComponent implements OnInit, OnDestroy {

  readingId: string;
  words: StudentWord[] = [];

  currentWord: StudentWord;
  currentWordIndex = 0;

  result = "";
  wordArray = [];
  showResult = false;

  sub: Subscription = new Subscription();

  constructor(
    private location: Location,
    private studentService: StudentService,
    private route: ActivatedRoute,
  ) { }

  ngOnInit() {
    this.sub.add(this.sub = this.route.params.subscribe(params => {
      this.readingId = params['readingId'];
      this.studentService.getPretestWords(this.readingId).subscribe(result => {
        this.words = result;
        if (this.words.length)
          this.setCurrentWord();
        else
          this.location.back();
      });
    }));
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  setCurrentWord() {
    if (this.words && this.words.length)
      this.currentWord = this.words[this.currentWordIndex];
  }

  handleAnswer(isCorrect) {
    if (isCorrect) {
      this.sub.add(this.studentService.setKnownWord(this.currentWord).subscribe(result => {
        this.nextWord();
      }));
    }
  }

  skipWord() {
    this.sub.add(this.studentService.addStudentWord(this.currentWord).subscribe(result => {
      this.nextWord();
    }));
  }

  nextWord() {
    if (this.words.length === this.currentWordIndex + 1) {
      this.endPretest();
      return;
    }

    this.currentWordIndex++
    this.currentWord = this.words[this.currentWordIndex];
  }

  endPretest() {
    this.location.back();
  }
}

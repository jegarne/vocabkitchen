import { Component, OnInit, Input, EventEmitter, Output, ViewChild, AfterViewInit } from '@angular/core';

@Component({
  selector: 'cloze',
  templateUrl: './cloze.component.html',
  styleUrls: ['./cloze.component.css']
})
export class ClozeComponent implements OnInit, AfterViewInit {
  @Input() sentences: string[];
  @Input() word: string;

  @Output() isCorrect: EventEmitter<boolean> = new EventEmitter<boolean>();

  @ViewChild('answer', { static: false }) answerInput;

  cloze = '';

  constructor() { }

  ngOnInit() {
    const randomIndex = Math.floor(Math.random() * this.sentences.length);
    let re = new RegExp(`\\b${this.word}\\b`, 'gi');
    this.cloze = this.sentences[randomIndex].replace(re, ' _____ ');
  }

  ngAfterViewInit() {
    this.answerInput.nativeElement.focus();
  }

  checkAnswer = () => {
    if (this.answerInput.nativeElement.value) {
      if (this.answerInput.nativeElement.value.toLowerCase() === this.word.toLowerCase()) {
        this.isCorrect.emit(true);
      }
    }
  }
}

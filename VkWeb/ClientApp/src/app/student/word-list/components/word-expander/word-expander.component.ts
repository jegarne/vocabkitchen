import { Component, OnInit, Input } from '@angular/core';
import { StudentWord } from '../../../models/student-word';

@Component({
  selector: 'word-expander',
  templateUrl: './word-expander.component.html',
  styleUrls: ['./word-expander.component.css']
})
export class WordExpanderComponent implements OnInit {

  @Input() words: StudentWord[];
  @Input() wordClass = '';

  constructor() { }

  ngOnInit() {
    this.words.forEach(w => {
      let re = new RegExp(`\\b${w.word}\\b`, 'gi');
      w.sentences = w.sentences.map(s => s.replace(re, `<b>${w.word}</b>`));
    });
  }
}

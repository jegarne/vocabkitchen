import { Component, OnInit, OnDestroy } from '@angular/core';
import { StudentService } from '@services/student.service';
import { StudentWord } from '@models/student-word';
import { Subscription } from 'rxjs';

@Component({
  selector: 'word-list',
  templateUrl: './word-list.component.html',
  styleUrls: ['./word-list.component.css']
})
export class WordListComponent implements OnInit {
  id: string;
  loading = true;

  knownWords: StudentWord[] = new Array<StudentWord>();
  newWords: StudentWord[] = new Array<StudentWord>();
  inProgressWords: StudentWord[] = new Array<StudentWord>();

  show = -1;

  private sub = new Subscription();
  constructor(
    private studentService: StudentService
  ) { }

  ngOnInit() {
    this.sub.add(this.studentService.getWords().subscribe(result => {
      this.knownWords = result.filter(w => w.isKnown);
      this.newWords = result.filter(w => !w.isKnown && w.attempts && w.attempts.length == 0);
      this.inProgressWords = result.filter(w => !w.isKnown && w.attempts && w.attempts.length > 0);

      if (this.newWords.length > 0) {
        this.show = 0;
      } else if (this.inProgressWords.length > 0) {
        this.show = 1;
      } else if (this.knownWords.length > 0) {
        this.show = 2;
      }; 

      this.loading = false;
    }));
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

}

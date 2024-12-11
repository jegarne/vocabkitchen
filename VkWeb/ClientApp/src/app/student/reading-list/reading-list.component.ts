import { Component, OnInit, OnDestroy } from '@angular/core';
import { StudentService } from '@services/student.service';
import { ReadingInfo } from '@models/org';
import { Subscription } from 'rxjs';

@Component({
  selector: 'reading-list',
  templateUrl: './reading-list.component.html',
  styleUrls: ['./reading-list.component.css']
})
export class ReadingListComponent implements OnInit, OnDestroy {
  id: string;
  readings: ReadingInfo[];

  private sub = new Subscription();

  constructor(
    private studentService: StudentService
  ) { }

  ngOnInit() {
    this.sub.add(this.studentService.getReadings().subscribe(result => {
      this.readings = result;
    }));
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

}

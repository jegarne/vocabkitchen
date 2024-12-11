import { Component, OnInit } from '@angular/core';
import { StudentService } from '@services/student.service';
import { Student } from '@models/org';
import { Subscription } from 'rxjs';

@Component({
  selector: 'student-nav',
  templateUrl: './student-nav.component.html',
  styleUrls: ['./student-nav.component.css']
})
export class StudentNavComponent implements OnInit {
  student: Student = new Student();
  private sub = new Subscription();

  constructor(
    private studentService: StudentService
  ) { }

  ngOnInit() {
    const sub = this.studentService.get('').subscribe(s => {
      this.student = s;
    });
    this.sub.add(sub);
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}

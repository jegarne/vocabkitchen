import { Component, OnInit, ViewChild, AfterViewInit} from '@angular/core';
import { Student, Tag } from '@models/org';
import { StudentService } from '@services/student.service';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Location } from '@angular/common';
import { OrgService } from '@services/org.service';
import { StudentProgress, WordProgress } from '@models/student-progress';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';


@Component({
  selector: 'student-details',
  templateUrl: './student-details.component.html',
  styleUrls: ['./student-details.component.css']
})
export class StudentDetailsComponent implements OnInit, AfterViewInit {
  orgId: string;
  studentId: string;
  student: Student = new Student();
  studentProgress: StudentProgress = new StudentProgress();

  words: MatTableDataSource<WordProgress> = new MatTableDataSource();
  displayedColumns: string[] = [
    'word',
    'status',
    'isKnownDate',
    'totalAttempts',
    'spellingFailPercentage',
    'clozeFailPercentage',
    'meaningFailPercentage'
  ];

  @ViewChild(MatSort, { static: true }) sort: MatSort;

  private sub: Subscription = new Subscription();

  constructor(
    private route: ActivatedRoute,
    private studentService: StudentService,
    private orgService: OrgService,
    private location: Location

    ) { }

  ngOnInit() {
    
    this.sub.add(this.route.params.subscribe(params => {
      this.studentId = params['studentId'];
      this.orgId = params['orgId'];
      this.studentService.get(this.studentId).subscribe(result => this.student = result);
      this.orgService.getStudentProgress(this.orgId, this.studentId)
        .subscribe(result => {
          this.studentProgress = result;
          this.words.data = result.words;

        });
    }));
  }

  ngAfterViewInit() {
    this.words.sort = this.sort;
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  back() {
    this.location.back();
  }

  addTag(newTag: Tag) {
    this.sub.add(this.studentService.postTag(this.studentId, newTag.id).subscribe(result =>
      this.student.tags = result));
  }

  removeTag(removedTag: Tag) {
    this.sub.add(this.studentService.deleteTag(this.studentId, removedTag.id).subscribe(result =>
      this.student.tags = result));
  }

}

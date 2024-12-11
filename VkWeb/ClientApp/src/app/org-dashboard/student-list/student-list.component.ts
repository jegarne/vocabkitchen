import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { OrgService } from '@services/org.service';
import { Student } from '@models/org';
import { combineLatest } from 'rxjs';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'student-list',
  templateUrl: './student-list.component.html',
  styleUrls: ['./student-list.component.css']
})
export class StudentListComponent implements OnInit, AfterViewInit, OnDestroy {
  orgId: string;
  private sub: any;
  students: MatTableDataSource<Student> = new MatTableDataSource();

  displayedColumns: string[] = ['firstName',
    'lastName', 'newReadingsCount', 'newWordsCount',
    'inProgressWordsCount', 'knownWordsCount', 'details'];

  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(
    private activatedRoute: ActivatedRoute,
    private orgService: OrgService
  ) { }

  ngOnInit() {

    const params = combineLatest(this.activatedRoute.params, this.activatedRoute.queryParams,
      (params, qparams) => ({ params, qparams }));

    this.sub = params.subscribe(ap => {
      this.orgId = ap.params['orgId'];
      this.orgService.getStudentsProgress(this.orgId).subscribe(result => {
        this.students.data = result;
      });
    });
  }

  ngAfterViewInit() {
    this.students.sort = this.sort;
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

}

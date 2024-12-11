import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { OrgService } from '@services/org.service';
import { WordAttemptSummary } from '@models/org';
import { combineLatest } from 'rxjs';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'word-list',
  templateUrl: './word-list.component.html',
  styleUrls: ['./word-list.component.css']
})
export class WordListComponent implements OnInit {
  orgId: string;
  private sub: any;
  words: MatTableDataSource<WordAttemptSummary> = new MatTableDataSource();
  error = '';

  displayedColumns: string[] = ['word',
    'spellingFailures', 'spellingAttempts', 'meaningFailures',
    'meaningAttempts', 'clozeFailures', 'clozeAttempts'];

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
      this.orgService.getWordsProgress(this.orgId).subscribe(result => {
        this.words.data = result;
      },
        () => {
          this.error = "Words will become available once students start studying.";
        });
    });
  }

  ngAfterViewInit() {
    this.words.sort = this.sort;
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}

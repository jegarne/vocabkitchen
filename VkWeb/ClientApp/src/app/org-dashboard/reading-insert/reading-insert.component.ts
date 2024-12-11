import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReadingService } from '@services/reading.service';
import { Reading } from '@models/reading';

@Component({
  selector: 'reading-insert',
  templateUrl: './reading-insert.component.html',
  styleUrls: ['./reading-insert.component.css']
})
export class ReadingInsertComponent implements OnInit {
  orgId: string;
  private sub: any;

  serverErrors: any;
  title: string;
  text: string;

  constructor(
    private route: ActivatedRoute,
    private readingService: ReadingService,
    private router: Router
  ) { }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.orgId = params['orgId'];
    });
  }

  save() {
    this.readingService.post(new Reading(this.orgId, this.title, this.text)).subscribe((reading) => {
      this.router.navigate(['/org/details', this.orgId, 'reading', 'details', reading.id]);
      },
      errors => {
        this.serverErrors = errors;
      });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

}

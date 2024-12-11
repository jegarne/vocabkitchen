import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { OrgService } from '@services/org.service';
import { Org } from '@models/org';
import { combineLatest } from 'rxjs';

@Component({
  selector: 'org-detail',
  templateUrl: './org-detail.component.html',
  styleUrls: ['./org-detail.component.css']
})
export class OrgDetailComponent implements OnInit, OnDestroy {
  orgId: string;
  private sub: any;
  org: Org = new Org();

  constructor(
    private activatedRoute: ActivatedRoute,
    private orgService: OrgService,
  ) { }

  ngOnInit() {
    const params = combineLatest(this.activatedRoute.params, this.activatedRoute.queryParams,
      (params, qparams) => ({ params, qparams }));

    this.sub = params.subscribe(ap => {
      this.orgId = ap.params['orgId'];
      this.orgService.get(this.orgId).subscribe(org => this.org = org);
    });
  }

  addTag(value: string) {
    this.orgService.addTag(this.orgId, value).subscribe(result => this.org.tags = result);
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

}

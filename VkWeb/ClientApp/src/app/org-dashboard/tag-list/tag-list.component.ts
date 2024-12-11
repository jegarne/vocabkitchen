import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { OrgService } from '@services/org.service';
import { Org } from '@models/org';
import { combineLatest } from 'rxjs';

@Component({
  selector: 'tag-list',
  templateUrl: './tag-list.component.html',
  styleUrls: ['./tag-list.component.css']
})
export class TagListComponent implements OnInit {
  orgId: string;
  private sub: any;
  org: Org = new Org();
  serverErrors: any;

  constructor(
    private activatedRoute: ActivatedRoute,
    private orgService: OrgService
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

  deleteTag(tagId: string) {
    this.orgService.deleteTag(tagId).subscribe(
      result => {
        this.org.tags = this.org.tags.filter(el => el.id !== result.id);
      },
      errors => {
        this.serverErrors = errors;
      });
  }

  toggleDefault(tagId: string) {
    console.log(tagId);
    this.orgService.toggleDefaultTag(tagId).subscribe(result => this.org.tags = result);
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

}

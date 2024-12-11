import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrgAdminService } from '@services/org-admin.service';
import { Organization } from '@models/organization';

@Component({
  selector: 'org-admin-detail',
  templateUrl: './org-admin-detail.component.html',
  styleUrls: ['./org-admin-detail.component.css']
})
export class OrgAdminDetailComponent implements OnInit, OnDestroy {
  orgId: string;
  private sub: any;
  org: Organization = new Organization();

  constructor(
    private route: ActivatedRoute,
    private orgService: OrgAdminService
  ) { }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.orgId = params['id'];
      this.orgService.get(this.orgId).subscribe(org => {
        this.org = org;
      });
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}

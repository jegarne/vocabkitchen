import { Component, OnInit } from '@angular/core';
import { Org } from '@models/org';
import { OrgService } from '@services/org.service';
import { Router } from '@angular/router';

@Component({
  selector: 'org-dashboard',
  templateUrl: 'org-dashboard.component.html',
  styleUrls: ['./org-dashboard.component.css']
})
export class OrgDashboardComponent implements OnInit {
  orgs: Org[];
  loading = true;
  defaultOrgId = '';

  constructor(
    private orgService: OrgService,
    private router: Router
  ) { }

  ngOnInit() {
    this.orgService.list().subscribe((result) => {
      if (result.length === 0) {
        this.router.navigate(['/student']);
      } else if (result.length === 1) {
        this.router.navigate(['/org/details', result[0].id]);
      } else {
        this.setOrgs(result);
        this.loading = false;
      }
    });
  }

  setOrgs(result) {
    this.orgs = result;
    const defaultOrg = this.orgs.find(r => r.isDefault);
    if (defaultOrg)
      this.defaultOrgId = defaultOrg.id;
  }

  onChange(newValue) {
    this.defaultOrgId = newValue;
    this.orgService.setDefaultOrg(this.defaultOrgId).subscribe(result => this.setOrgs(result));
  }
}

import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OrgAdminService } from '@services/org-admin.service';
import { Organization } from './models/organization';

@Component({
  selector: 'org-admin',
  templateUrl: './org-admin.component.html'
})
export class OrgAdminComponent implements OnInit {
  orgs: Organization[];
  loading = true;

  constructor(
    private orgService: OrgAdminService,
    private router: Router
  ) { }

  ngOnInit() {
    this.orgService.list().subscribe((result) => {
      if (result.length === 0) {
        this.router.navigate(['/org']);
      } else if (result.length === 1) {
        this.router.navigate(['/org-admin/details', result[0].id]);
      } else {
        this.orgs = result;
        this.loading = false;
      }
    });
  }
}

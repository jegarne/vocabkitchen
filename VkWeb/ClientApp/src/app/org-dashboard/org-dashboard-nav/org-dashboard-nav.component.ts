import { Component, Input } from '@angular/core';

@Component({
  selector: 'org-dashboard-nav',
  templateUrl: './org-dashboard-nav.component.html',
  styleUrls: ['./org-dashboard-nav.component.css']
})
export class OrgDashboardNavComponent {
  @Input() id: string;
  @Input() title: string;
}

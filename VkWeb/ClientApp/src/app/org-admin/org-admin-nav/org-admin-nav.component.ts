import { Component, Input } from '@angular/core';

@Component({
  selector: 'org-admin-nav',
  templateUrl: './org-admin-nav.component.html',
  styleUrls: ['./org-admin-nav.component.css']
})
export class OrgAdminNavComponent {
  @Input() id: string;
  @Input() title: string;
}

import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ContentItem } from '@models/reading';



@Component({
  selector: 'definition-modal',
  templateUrl: './definition-modal.component.html',
  styleUrls: ['./definition-modal.component.css']
})
export class DefinitionModalComponent {
  contentItem: ContentItem;

  constructor(
    public bsModalRef: BsModalRef,
  ) { }
}

import { Component, OnInit, OnDestroy, ElementRef, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ReadingService } from '@services/reading.service';
import { Subscription } from 'rxjs/Subscription';

import { TextSelectEvent } from "./text-select.directive";
import { Definition, Reading } from '@models/reading';
import { Tag } from '@models/org';

import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { DefinitionModalComponent } from '../definition-modal/definition-modal.component';

interface ContentItemRange {
  contentItemStartIndex: number;
  contentItemEndIndex: number;
  start: number;
  end: number;
}

@Component({
  selector: 'reading-details',
  templateUrl: './reading-details.component.html',
  styleUrls: ['./reading-details.component.css']
})
export class ReadingDetailsComponent implements OnInit, OnDestroy {
  orgId: string;
  readingId: string;
  private subscriptions: Subscription[] = [];
  reading: Reading = new Reading(this.orgId, "", "");

  contentItemRange: ContentItemRange | null;
  selectedText: string;

  @ViewChild('definitionInput', { read: ElementRef, static: false }) definitionInput: ElementRef;
  bsModalRef: BsModalRef;

  constructor(
    private route: ActivatedRoute,
    private readingService: ReadingService,
    private modalService: BsModalService,
    private router: Router,

  ) { }

  ngOnInit() {
    this.loadReading();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  public loadReading() {
    this.subscriptions.push(this.route.params.subscribe(params => {
      this.orgId = params['orgId'];
      this.readingId = params['readingId'];
      this.readingService.get(this.readingId).subscribe(reading => this.reading = reading);
    }));
  }

  public renderRectangles(event: TextSelectEvent): void {

    if (this.selectedText) return;

    if (event.hostRectangle) {
      this.contentItemRange = event.contentItemRange;
      this.selectedText = event.text;
      this.setDefinition();
    }
  }

  setDefinition() {
    let definition = new Definition(
      this.readingId,
      this.contentItemRange.contentItemStartIndex,
      this.contentItemRange.contentItemEndIndex,
      this.contentItemRange.start,
      this.contentItemRange.end,
      this.selectedText
    );
    const initialState = { readingId: this.readingId, contentItem: null, newDefinition: definition };
    this.handleDefinitionModal(initialState);
  }

  showDefinition(item) {
    if (item.definition) {
      const initialState = { readingId: this.readingId, contentItem: item, definition: null };
      this.handleDefinitionModal(initialState);
    }
  }

  private handleDefinitionModal(initialState) {
    this.bsModalRef = this.modalService.show(DefinitionModalComponent, { initialState });
    this.bsModalRef.content.onClose.subscribe(result => {
      this.clearSelection();
      if (result) {
        this.loadReading();
      }
    });
  }

  clearSelection() {
    document.getSelection().removeAllRanges();
    this.contentItemRange = null;
    this.selectedText = "";
  }

  addTag(newTag: Tag) {
    this.subscriptions.push(this.readingService.postTag(this.readingId, newTag.id).subscribe(result =>
      this.reading.tags = result));
  }

  removeTag(removedTag: Tag) {
    this.subscriptions.push(this.readingService.deleteTag(this.readingId, removedTag.id).subscribe(result =>
      this.reading.tags = result));
  }

  delete() {
    this.subscriptions.push(this.readingService.delete(this.readingId).subscribe(result => {
      this.router.navigate(['/org', 'details', this.orgId]);
    }));
  }
}

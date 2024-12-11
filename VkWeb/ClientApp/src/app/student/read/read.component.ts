import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ReadingService } from '@services/reading.service';
import { Reading } from '@models/reading';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { DefinitionModalComponent } from '../definition-modal/definition-modal.component';

@Component({
  selector: 'read',
  templateUrl: './read.component.html',
  styleUrls: ['./read.component.css']
})
export class ReadComponent implements OnInit, OnDestroy {
  readingId: string;
  private sub: any;
  reading: Reading = new Reading('', '', '');

  bsModalRef: BsModalRef;

  constructor(
    private activatedRoute: ActivatedRoute,
    private readingService: ReadingService,
    private router: Router,
    private modalService: BsModalService
  ) { }

  ngOnInit() {
    this.sub = this.activatedRoute.params.subscribe(params => {
      this.readingId = params['readingId'];
      this.readingService.get(this.readingId).subscribe(result => {
        this.reading = result;
        if (this.reading.newWords) {
          this.router.navigate(['/student/pretest', this.reading.id]);
        }
      });
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  showDefinition(item) {
    if (item.definition) {
      const initialState = { canEdit: false, readingId: this.readingId, contentItem: item };
      this.bsModalRef = this.modalService.show(DefinitionModalComponent, { initialState });
    }
  }
}

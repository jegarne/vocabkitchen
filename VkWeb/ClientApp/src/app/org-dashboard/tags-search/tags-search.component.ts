import {
  Component, Input, OnInit, Output, EventEmitter,
  OnDestroy, SimpleChanges, SimpleChange, ViewChild, ElementRef
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { Tag } from '@models/org';
import { OrgService } from '@services/org.service';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatAutocomplete, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatChipInputEvent } from '@angular/material/chips';

@Component({
  selector: 'tags-search',
  templateUrl: './tags-search.component.html',
  styleUrls: ['./tags-search.component.css']
})
export class TagsSearchComponent implements OnInit, OnDestroy {
  @Input() orgId: string;
  @Input() existingTags: Tag[];

  @Output() newTag = new EventEmitter<Tag>();
  @Output() removedTag = new EventEmitter<Tag>();

  isEditing = false;
  allTags: Tag[] = new Array<Tag>();
  availableTags: Tag[] = new Array<Tag>();
  tagCtrl = new FormControl();
  filteredTags: Observable<Tag[]>;

  separatorKeysCodes: number[] = [ENTER, COMMA];

  @ViewChild('tagInput', { static: false }) tagInput: ElementRef<HTMLInputElement>;
  @ViewChild('auto', { static: false }) matAutocomplete: MatAutocomplete;

  private subscriptions: Subscription[] = [];

  get isEnabled() {
    return this.tagCtrl.dirty && this.tagCtrl.valid;
  }

  constructor(
    private orgService: OrgService
  ) { }

  ngOnInit() {

    this.subscriptions.push(this.orgService.searchTags(this.orgId, "").subscribe(
      result => {
        this.allTags = result;
        this.setAvailableTags();
      }));

    this.filteredTags = this.tagCtrl.valueChanges
      .pipe(
        startWith(''),
        map(value => this._filter(value))
      );
  }

  ngOnDestroy() {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  ngOnChanges(changes: SimpleChanges) {
    const changedTags: SimpleChange = changes.existingTags;
    this.existingTags = changedTags.currentValue;
    this.setAvailableTags();
  }

  displayFn(tag?: Tag): string | undefined {
    return tag ? tag.value : undefined;
  }

  edit() {
    this.isEditing = !this.isEditing;
  }

  setAvailableTags() {
    if (this.existingTags) {
      this.availableTags = this.allTags.filter(el =>
        !this.existingTags.some((f =>
            f.id === el.id) as any
        ));
    } else {
      this.availableTags = this.allTags;
    }
  }

  add(event: MatChipInputEvent): void {
    if (!this.matAutocomplete.isOpen) {
      const input = event.input;

      if (this.tagCtrl.value && typeof this.tagCtrl.value === 'string') {

        // create new tag and add it
        var tagValue = this.tagCtrl.value;
        this.orgService.addTag(this.orgId, tagValue).subscribe(result => {
          var newTag = result.find(x => x.value === tagValue);
          this.newTag.emit(newTag);
        });

      } else if (this.tagCtrl.value) {
        // add existing tag
        this.newTag.emit(this.tagCtrl.value);
      }

      if (input) {
        input.value = '';
      }

      this.tagCtrl.setValue(null);
    }
  }

  selected(event: MatAutocompleteSelectedEvent): void {
    this.existingTags.push(this.tagCtrl.value);
    this.tagInput.nativeElement.value = '';
    //this.tagCtrl.setValue(null);
    this.setAvailableTags();
  }

  remove(tag: any): void {
    this.removedTag.emit(tag);
  }

  private _filter(tag: any): Tag[] {
    let filterValue = '';

    if (!tag) {
      return this.availableTags;
    }

    if (tag.hasOwnProperty('value')) {
      filterValue = tag.value.toLowerCase();
    } else {
      filterValue = tag.toLowerCase();
    }

    return this.availableTags.filter(option =>
      option.value.toLowerCase().includes(filterValue));
  }

}

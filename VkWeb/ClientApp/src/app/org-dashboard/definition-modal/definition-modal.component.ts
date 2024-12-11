import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

import { BsModalRef } from 'ngx-bootstrap/modal';
import { ContentItem, Definition } from '@models/reading';
import { Subject } from 'rxjs/Subject';
import { ReadingService } from '@services/reading.service';
import { WordService } from '@services/word.service';
import { Subscription } from 'rxjs/Subscription';
import { SuggestedDefinition, DefinitionSource } from '@models/reading';
import { Observable, BehaviorSubject } from 'rxjs/Rx';
import { combineLatest } from 'rxjs';
import { distinctUntilChanged, publishReplay, refCount } from 'rxjs/operators';

@Component({
  selector: 'definition-modal',
  templateUrl: './definition-modal.component.html',
  styleUrls: ['./definition-modal.component.scss']
})
export class DefinitionModalComponent implements OnInit, OnDestroy {
  readingId: string;
  word: string;
  definition: string;
  partOfSpeech: string;
  annotationId: string;
  source = 'user';
  isValid = true;
  isDefinitionEmpty = false;

  // passed into modal
  contentItem: ContentItem;
  newDefinition: Definition;

  onClose: Subject<boolean>;
  definitions$: Observable<SuggestedDefinition[]>;

  private subscription: Subscription;
  form: FormGroup;
  selectedSourceCode$ = new BehaviorSubject('user');
  filteredSources$ = new Subject<DefinitionSource[]>();
  filteredDefinitions$ = new Subject<SuggestedDefinition[]>();
  attributionText$ = new BehaviorSubject('');

  @ViewChild('definitionInput', { static: false }) nameInputRef: ElementRef;

  constructor(
    public bsModalRef: BsModalRef,
    private readingService: ReadingService,
    private wordService: WordService,
    private formBuilder: FormBuilder
  ) {
    this.form = this.formBuilder.group({
      source: ''
    });
  }

  ngOnInit(): void {
    this.onClose = new Subject();
    this.subscription = new Subscription();

    if (this.newDefinition) {
      this.word = this.newDefinition.word;
      this.definition = "";

      if (this.word.length > 50) {
        this.isValid = false;
        return;
      }

      this.loadDefinitions();
    } else {
      this.word = this.contentItem.value;
      this.definition = this.contentItem.definition;
    }
  }

  ngOnDestroy() {
    this.onClose.next(false);
    this.subscription.unsubscribe();
  }

  // saves contents of text area
  public saveDefinition() {
    this.newDefinition.definition = this.nameInputRef.nativeElement.value;
    // todo: add logic to see if the definition has actually changed

    if (!this.newDefinition.definition) {
      this.isDefinitionEmpty = true;
      return;
    }

    this.isDefinitionEmpty = false;
    this.newDefinition.source = this.source;
    this.newDefinition.annotationId = this.annotationId;
    this.subscription.add(
      this.readingService.postDefinition(this.newDefinition).subscribe(result => { this.wasUpdated(); })
    );
  }

  // saves 3rd part definition directly
  public saveExternalDefinition(def: SuggestedDefinition) {
    this.newDefinition.definition = def.value;
    this.newDefinition.partOfSpeech = def.partOfSpeech;
    this.newDefinition.source = def.source;
    this.subscription.add(
      this.readingService.postDefinition(this.newDefinition).subscribe(result => { this.wasUpdated(); })
    );
  }

  public editExternalDefinition(def: SuggestedDefinition) {
    this.definition = def.value;
    this.partOfSpeech = def.partOfSpeech;
    this.annotationId = def.annotationId;
  }

  public removeDefinition() {
    this.subscription.add(
      this.readingService.removeDefinition(this.readingId, this.contentItem.id).subscribe(result => { this.wasUpdated(); })
    );
  }

  public deleteDefinition(def: SuggestedDefinition) {
    this.subscription.add(
      this.wordService.deleteDefinition(def.annotationId).subscribe(result => { this.resetDefinitions(); })
    );
  }

  public wasUpdated(): void {
    this.onClose.next(true);
    this.bsModalRef.hide();
  }

  private resetDefinitions() {
    this.definitions$ = null;
    this.loadDefinitions();
  }

  private loadDefinitions() {
    this.definitions$ = this.wordService.getDefinitions(this.word).pipe(
      publishReplay(1),
      refCount()
    );

    // populate dropdown with sources and set initial selected value
    this.subscription.add(combineLatest(this.wordService.getDefinitionSources(), this.definitions$)
      .subscribe(([sources, definitions]) => {

        const distinctSourceCodes = Array.from(new Set(definitions.map((item: any) => item.source)));
        const filteredSources = sources.filter((elem) => distinctSourceCodes.find((code) => elem.code === code));
        this.filteredSources$.next(filteredSources);

        if (distinctSourceCodes.length > 0) {
          const newCode = distinctSourceCodes[0];
          this.selectedSourceCode$.next(newCode);
          this.form.controls.source.patchValue(newCode);
        }
      }));

    // change displayed definitions and attribution text when dropdown is changed
    this.subscription.add(combineLatest(this.selectedSourceCode$, this.definitions$, this.filteredSources$)
      .subscribe(([sourceCode, definitions, sources]) => {
        const filteredDefinitions = definitions.filter((elem) => elem.source === sourceCode);
        this.filteredDefinitions$.next(filteredDefinitions);

        const selectedSource = sources.find(s => s.code === sourceCode);
        if (selectedSource)
          this.attributionText$.next(selectedSource.attributionText);

      }));

    // listen for dropdown changes
    this.subscription.add(this.form.valueChanges
      .pipe(distinctUntilChanged())
      .subscribe(code => {
        this.selectedSourceCode$.next(code.source);
      }));
  }
}

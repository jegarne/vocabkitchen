import { Component, HostListener, ElementRef, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { fromEvent, Subscription } from 'rxjs';
import { Edit } from '@models/edit';

import { ReadingService } from '@services/reading.service';
import { Reading } from '@models/reading';


@Component({
  selector: 'reading-edit',
  templateUrl: './reading-edit.component.html',
  styleUrls: ['./reading-edit.component.css']
})
export class ReadingEditComponent implements OnInit, OnDestroy {

  private editType = {
    INSERT: 'insert',
    DELETE: 'delete',
    PASTE: 'paste',
  }

  orgId: string;
  readingId: string;
  reading: Reading = new Reading(this.orgId, "", "");
  serverErrors: any;

  edits: Edit[] = new Array();
  redoStack: Edit[] = new Array();
  editIndex = -1;

  selectionRange = null;
  currentDelete = '';
  currentDeleteStartIndex: number = null;
  currentDeleteEndIndex: number = null;

  currentEdit = '';
  currentEditStartIndex: number = null;
  currentEditEndIndex: number = null;


  private subscriptions: Subscription[] = [];

  @ViewChild("myTextArea", { read: ElementRef, static: false }) myTextArea: ElementRef;

  constructor(
    private route: ActivatedRoute,
    private readingService: ReadingService,
    private router: Router
  ) { }

  ngAfterViewInit() {
    // dragstart
    this.myTextArea.nativeElement.addEventListener("dragstart", function (e) {
      e.preventDefault();
    });

    // blur
    const onBlurSource = fromEvent(this.myTextArea.nativeElement, 'blur');
    const onBlurSubscribe = onBlurSource.subscribe(evt => {
      this.saveCurrentState();
    });
    this.subscriptions.push(onBlurSubscribe);

    // onselect
    const onSelectSource = fromEvent(this.myTextArea.nativeElement, 'select');
    const onSelectSubscribe = onSelectSource.subscribe(evt => {
      this.setSelectionRange();
    });
    this.subscriptions.push(onSelectSubscribe);

    // keydown
    const keyDownSource = fromEvent(this.myTextArea.nativeElement, 'keydown');
    const keyDownSubscribe = keyDownSource.subscribe(evt => this.handleKeyboardEvent(evt));
    this.subscriptions.push(keyDownSubscribe);

    // click
    const clickSource = fromEvent(this.myTextArea.nativeElement, 'click');
    const clickSubscribe = clickSource.subscribe(evt => this.handleMouseEvent());
    this.subscriptions.push(clickSubscribe);

    // cut
    const cutSource = fromEvent(this.myTextArea.nativeElement, 'cut');
    const cutSubscribe = cutSource.subscribe((e: KeyboardEvent) => {
      this.saveCurrentState();
      let t = <HTMLTextAreaElement>e.target;
      let cutText = this.myTextArea.nativeElement.value.substr(t.selectionStart, t.selectionEnd - t.selectionStart);
      this.pushEdit(this.editType.DELETE, t.selectionStart, t.selectionEnd, cutText);
    });
    this.subscriptions.push(cutSubscribe);

    // paste
    const pasteSource = fromEvent(this.myTextArea.nativeElement, 'paste');
    const pasteSubscribe = pasteSource.subscribe((e: ClipboardEvent) => {
      this.saveCurrentState();
      let t = <HTMLTextAreaElement>e.target;
      // capture value
      const pastedText = e.clipboardData.getData('text/plain');
      this.pushEdit(this.editType.PASTE, t.selectionStart, t.selectionEnd, pastedText);
      // clear selection
      this.selectionRange = null;
    });
    this.subscriptions.push(pasteSubscribe);
  }


  ngOnInit(): void {

    const routeSub = this.route.params.subscribe(params => {
      this.orgId = params['orgId'];
      this.readingId = params['readingId'];
      this.readingService.get(this.readingId).subscribe(reading => this.reading = reading);
    });
    this.subscriptions.push(routeSub);

  };

  ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  save() {
    this.saveCurrentState();
    const readingSub = this.readingService.put(this.readingId, this.reading.title, this.edits).subscribe((reading) => {
        this.router.navigate(['/org/details', this.orgId, 'reading', 'details', reading.id]);
      },
      errors => {
        this.serverErrors = errors;
      });
    this.subscriptions.push(readingSub);
  }

  clear() {
    this.ngOnDestroy();
    this.ngOnInit();

    this.serverErrors = null;

    this.edits = new Array();
    this.redoStack = new Array();
    this.editIndex = -1;

    this.selectionRange = null;
    this.currentDelete = '';
    this.currentDeleteStartIndex = null;
    this.currentDeleteEndIndex = null;

    this.currentEdit = '';
    this.currentEditStartIndex = null;
    this.currentEditEndIndex = null;
  }

  cancel() {
    this.router.navigate(['/org/details', this.orgId, 'reading', 'details', this.readingId]);
  }

  // listen globally in case the select mouseup
  // happens outside the text area
  @HostListener('document:mouseup ', ['$event'])
  private handleMouseEvent() {
    this.setSelectionRange();
  }

  // listen globally in case ctrl-z / ctrl-y
  // happens outside the text area
  @HostListener('document:keydown ', ['$event'])
  private handleGlobalKeydown(event) {
    if (this.isUndo(event)) {
      this.handleUndo();
      return;
    }

    if (this.isRedo(event)) {
      this.handleRedo();
      return;
    }
  }

  private handleKeyboardEvent(event) {
    if (this.isCharacter(event)) {
      this.saveCurrentDelete();
      this.updateCurrentEdit(event.target.selectionStart, event.key);
      return;
    }

    if (this.isDelete(event)) {
      this.saveCurrentEdit();
      this.updateCurrentDeletion(event.target.selectionStart);
      return;
    }

    if (this.isBackspace(event)) {
      const start = event.target.selectionStart;
      const end = event.target.selectionEnd;
      if (start === end && start === 0) return;
      this.saveCurrentEdit();
      this.updateCurrentDeletion(start - 1);
      return;
    }
  }

  handleUndo() {
    this.saveCurrentState();
    let lastEdit = this.edits.find(x => x.index === this.editIndex);
    if (lastEdit) {
      let lastEditClone = Object.assign(lastEdit);
      this.redoStack.push(lastEditClone);

      this.edits = this.edits.filter(x => x.index !== this.editIndex);
      this.editIndex--;

      this.undoEditInText(lastEdit);
    }
  }

  handleRedo() {
    this.saveCurrentState();
    if (this.redoStack.length > 0) {
      let redo = this.redoStack.pop();
      this.edits.push(redo);
      this.editIndex++;
      this.applyEditToText(redo);
    }
  }

  private updateCurrentEdit(index: number, char: string) {
    char = this.mapCharacters(char);
    if (this.currentEditStartIndex === null) {
      // first entry
      this.currentEdit += char;
      this.currentEditStartIndex = index;
      this.currentEditEndIndex = index;
    } else if (this.currentEditEndIndex === index - 1) {
      // consecutive entry
      this.currentEdit += char;
      this.currentEditEndIndex = index;
    } else {
      // dump off current edit and start a new one
      this.pushEdit(this.editType.INSERT, this.currentEditStartIndex, this.currentEditEndIndex, this.currentEdit);
      this.currentEdit = char;
      this.currentEditStartIndex = index;
      this.currentEditEndIndex = index;
    }
  }

  private updateCurrentDeletion(index: number) {

    // handle deleted selections
    if (this.selectionRange) {
      let selectedText = this.myTextArea.nativeElement.value.substr(this.selectionRange.start, this.selectionRange.end - this.selectionRange.start);
      this.pushEdit(this.editType.DELETE, this.selectionRange.start, this.selectionRange.end - 1, selectedText);
      this.selectionRange = null;
      return;
    }

    // handle individual character deletions
    if (this.currentDeleteStartIndex === null) {
      // first entry
      this.currentDeleteStartIndex = index;
      this.currentDeleteEndIndex = index;
      this.currentDelete = this.myTextArea.nativeElement.value.substr(index, 1);
    } else if (this.currentDeleteStartIndex - 1 === index) {
      // push deletion to the left
      this.currentDeleteStartIndex--;
      this.currentDelete = this.myTextArea.nativeElement.value.substr(index, 1) + this.currentDelete;
    } else if (this.currentDeleteStartIndex === index) {
      // push deletion to the right
      this.currentDeleteEndIndex++;
      this.currentDelete = this.currentDelete + this.myTextArea.nativeElement.value.substr(index, 1);
    } else {
      // dump off current deletions range and start a new one
      this.pushEdit(this.editType.DELETE, this.currentDeleteStartIndex, this.currentDeleteEndIndex, this.currentDelete);
      this.currentDeleteStartIndex = index;
      this.currentDeleteEndIndex = index;
      this.currentDelete = this.myTextArea.nativeElement.value.substr(index, 1);
    }
  }

  private pushEdit(type, start, end, value) {
    this.editIndex++;
    let newEdit = new Edit(this.editIndex, type, start, end, value);
    this.edits.push(newEdit);
    // any new edits will replace old edits in the redo stack
    // with same index so it's safe to clear them
    this.redoStack = this.redoStack.filter(x => x.index !== this.editIndex);
  }

  private applyEditToText(edit: Edit) {
    if (edit.type === this.editType.PASTE || edit.type === this.editType.INSERT) {
      var a = this.myTextArea.nativeElement.value;
      this.myTextArea.nativeElement.value = [a.slice(0, edit.start), edit.value, a.slice(edit.start)].join('');
      return;
    }

    if (edit.type === this.editType.DELETE) {
      var a = this.myTextArea.nativeElement.value;
      this.myTextArea.nativeElement.value = a.substr(0, edit.start) + a.substr(edit.start + edit.value.length);
      return;
    }
  }

  private undoEditInText(edit: Edit) {
    if (edit.type === this.editType.PASTE || edit.type === this.editType.INSERT) {
      var a = this.myTextArea.nativeElement.value;
      this.myTextArea.nativeElement.value = a.substr(0, edit.start) + a.substr(edit.start + edit.value.length);
      return;
    }

    if (edit.type === this.editType.DELETE) {
      var a = this.myTextArea.nativeElement.value;
      this.myTextArea.nativeElement.value = [a.slice(0, edit.start), edit.value, a.slice(edit.start)].join('');
    }
  }

  private saveCurrentState() {
    this.saveCurrentEdit();
    this.saveCurrentDelete();
  }

  private saveCurrentEdit() {
    if (this.currentEdit) {
      this.pushEdit(this.editType.INSERT, this.currentEditStartIndex, this.currentEditEndIndex, this.currentEdit);
      this.currentEdit = '';
      this.currentEditStartIndex = null;
      this.currentEditEndIndex = null;
    }
  }

  private saveCurrentDelete() {
    if (this.currentDelete) {
      this.pushEdit(this.editType.DELETE, this.currentDeleteStartIndex, this.currentDeleteEndIndex, this.currentDelete);
      this.currentDeleteStartIndex = null;
      this.currentDeleteEndIndex = null;
      this.currentDelete = '';
    }
  }

  private setSelectionRange() {
    if (this.myTextArea.nativeElement.selectionStart !== this.myTextArea.nativeElement.selectionEnd) {
      this.selectionRange = {
        start: this.myTextArea.nativeElement.selectionStart,
        end: this.myTextArea.nativeElement.selectionEnd
      };
    };
  }

  private isUndo(event: KeyboardEvent) {
    if (event.ctrlKey && event.keyCode === 90) {
      event.preventDefault();
      return true;
    }
    return false;
  }

  private isRedo(event: KeyboardEvent) {
    if (event.ctrlKey && event.keyCode === 89) {
      event.preventDefault();
      return true;
    }
    return false;
  }

  private mapCharacters(char: string) {
    const newLine = "\r\n";
    switch (char) {
      case "Enter":
        return newLine;
      default:
        return char;
    }
  }

  private isDelete(event: KeyboardEvent) {
    if (this.deleteKeyCodes.hasOwnProperty(event.keyCode)) return true;

    return false;
  }

  private deleteKeyCodes = {
    46: 'delete'
  }

  private isBackspace(event: KeyboardEvent) {
    if (this.backspaceKeyCodes.hasOwnProperty(event.keyCode)) return true;

    return false;
  }

  private backspaceKeyCodes = {
    8: 'backspace / delete',
  }

  private isCharacter(event: KeyboardEvent) {
    if (event.ctrlKey) return false;
    if (event.altKey) return false;
    if (this.charKeyCodes.hasOwnProperty(event.keyCode)) return true;

    return false;
  }

  private charKeyCodes = {
    13: 'enter',
    32: 'spacebar',
    48: '0',
    49: '1',
    50: '2',
    51: '3',
    52: '4',
    53: '5',
    54: '6',
    55: '7',
    56: '8',
    57: '9',
    58: ':',
    59: 'semicolon (firefox), equals',
    60: '<',
    61: 'equals (firefox)',
    63: 'ß',
    64: '@ (firefox)',
    65: 'a',
    66: 'b',
    67: 'c',
    68: 'd',
    69: 'e',
    70: 'f',
    71: 'g',
    72: 'h',
    73: 'i',
    74: 'j',
    75: 'k',
    76: 'l',
    77: 'm',
    78: 'n',
    79: 'o',
    80: 'p',
    81: 'q',
    82: 'r',
    83: 's',
    84: 't',
    85: 'u',
    86: 'v',
    87: 'w',
    88: 'x',
    89: 'y',
    90: 'z',
    96: 'numpad 0',
    97: 'numpad 1',
    98: 'numpad 2',
    99: 'numpad 3',
    100: 'numpad 4',
    101: 'numpad 5',
    102: 'numpad 6',
    103: 'numpad 7',
    104: 'numpad 8',
    105: 'numpad 9',
    106: 'multiply',
    107: 'add',
    108: 'numpad period (firefox)',
    109: 'subtract',
    110: 'decimal point',
    111: 'divide',
    160: '^',
    161: '!',
    162: '؛ (arabic semicolon)',
    163: '#',
    164: '$',
    165: 'ù',
    170: '*',
    171: '~ + * key',
    186: 'semi-colon / ñ',
    187: 'equal sign',
    188: 'comma',
    189: 'dash',
    190: 'period',
    191: 'forward slash / ç',
    192: 'grave accent / ñ / æ / ö',
    193: '?, / or °',
    194: 'numpad period (chrome)',
    219: 'open bracket',
    220: 'back slash',
    221: 'close bracket / å',
    222: 'single quote / ø / ä',
    223: '`',
  }
}

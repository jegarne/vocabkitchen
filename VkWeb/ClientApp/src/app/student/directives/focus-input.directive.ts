import { Directive, ElementRef, Input, SimpleChanges, EventEmitter } from '@angular/core';
import { Subject } from 'rxjs';

@Directive({
  selector: '[focusInput]'
})
export class FocusInputDirective {

  @Input('myFocus') isFocused: boolean;
  @Input('myValue') inputValue = '';
  focusEvent = new Subject<boolean>();
  blurEvent = new Subject<boolean>();

  constructor(private hostElement: ElementRef) {
    this.focusEvent.subscribe(event => {
      this.focus();
    });
  }

  ngOnInit() {
    this.hostElement.nativeElement.value = this.inputValue.toLowerCase();
  }

  ngAfterViewInit() {

  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.isFocused && changes.isFocused.currentValue)
      this.focus();

    if (changes.inputValue) {
      this.hostElement.nativeElement.value = changes.inputValue.currentValue.toLowerCase();
    }
  }

  focus() {      
    this.hostElement.nativeElement.select();
    const length = this.hostElement.nativeElement.value.length;
    this.hostElement.nativeElement.setSelectionRange(length, length);

    // hack to keep focus on input in safari
    setTimeout(() => {
      let event = new KeyboardEvent('keydown', {
        'bubbles': true, 'key': 'Backspace', 'code':'focusInput' });
        this.hostElement.nativeElement.dispatchEvent(event);
    }, 0);
  }
}

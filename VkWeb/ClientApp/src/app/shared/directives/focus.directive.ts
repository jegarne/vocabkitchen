import { Directive, ElementRef, OnInit } from "@angular/core";

@Directive({ selector: '[tmFocus]' })

export class myFocus implements OnInit {
  constructor(private el: ElementRef) {
    // focus won't work at construction time - too early
  }

  ngOnInit() {
    this.el.nativeElement.focus();
  }
}

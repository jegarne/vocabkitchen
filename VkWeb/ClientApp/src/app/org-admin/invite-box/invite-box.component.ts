import { Component, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'invite-box',
  templateUrl: './invite-box.component.html',
  styleUrls: ['./invite-box.component.css']
})
export class InviteBoxComponent {

  @Output() updateEmails: EventEmitter<string[]> = new EventEmitter<string[]>();

  getEmails(input) {
    let output = [];
    let lines = String(input).split(/\n/);
    for (var i = 0; i < lines.length; i++) {
      // only push this line if it contains a non whitespace character.
      if (/\S/.test(lines[i])) {
        output.push(lines[i].trim());
      }
    }
    this.updateEmails.next(output);
  }

}

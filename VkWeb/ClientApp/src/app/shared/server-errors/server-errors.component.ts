import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'server-errors',
  templateUrl: './server-errors.component.html',
  styleUrls: ['./server-errors.component.css']
})
export class ServerErrorsComponent implements OnChanges {

  @Input() serverErrors;
  objectKeys = Object.keys;

  errors: string[] = [];
  showErrors = false;

  ngOnChanges(changes: SimpleChanges) {
    this.errors = [];
    this.showErrors = false;

    if (changes.serverErrors) {
      let response = changes.serverErrors.currentValue as HttpErrorResponse;
      if (response && response.error) {
        this.showErrors = true;
        Object.keys(response.error).forEach(item => {
          this.errors.push(response.error[item]); // value
        });
      } else if (response) {
        this.showErrors = true;
        this.errors.push("Something didn't work.  Please use the feedback link about to tell use about the problem."); // value
      }
    }
  }
}

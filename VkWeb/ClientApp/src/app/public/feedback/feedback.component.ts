import { Component, AfterViewInit, Inject } from '@angular/core';
import { NgForm } from '@angular/forms';
import { FeedbackService } from '../../core/services/feedback.service';

@Component({
  selector: 'feedback',
  templateUrl: './feedback.component.html',
  styleUrls: ['./feedback.component.css']
})
export class FeedbackComponent {

  wasSubmitted = false;

  constructor(private feedbackService: FeedbackService) { }

  onSubmit(form: NgForm) {
    this.feedbackService.sendFeedback(form.value).subscribe(r => {
      this.wasSubmitted = true;
    });
  }

}

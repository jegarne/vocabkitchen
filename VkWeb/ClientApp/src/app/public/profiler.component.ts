import { Component, OnInit, ElementRef, Renderer2 } from '@angular/core';
import { ProfilerService } from '@services/profiler.service';
import { ProfilerRequest } from '@models/profiler-request';
import { Subscription } from 'rxjs';
import { AuthService } from '@services/auth.service';
import { AccessService } from '@services/access.service';
import { tap } from 'rxjs/operators';
import { ReadingService } from '@services/reading.service';
import { Router } from '@angular/router';

@Component({
  selector: 'profiler',
  templateUrl: './profiler.component.html',
  styleUrls: ['./profiler.component.css']
})
export class ProfilerComponent implements OnInit {
  profileResult: ProfilerRequest;
  profilerInput: string;
  objectKeys = Object.keys;
  isCefr = false;
  isNawl = false;
  isLoggedIn: boolean;
  isTeacher = false;
  hasSignInAlert = true;

  sub = new Subscription();

  constructor(
    private profilerService: ProfilerService,
    private userService: AuthService,
    private accessService: AccessService,
    private readingService: ReadingService,
    private router: Router,
    private elem: ElementRef,
    private renderer: Renderer2
  ) { }

  ngOnInit() {
    const authSub = this.userService.authNavStatus$.pipe(
      tap(
        loggedIn => {
          if (loggedIn)
            this.accessService.get().subscribe(result => {
              this.isTeacher = result.isTeacher;
            });
        }
      )).subscribe(result => this.isLoggedIn = result);

    this.sub.add(authSub);
  }

  submitCefr() {
    const request = new ProfilerRequest('cefr', this.profilerInput);
    this.submitToProfiler(request);
    this.isCefr = true;
    this.scrollToTop();

  }

  downloadCefrDoc() {
    const request = new ProfilerRequest('cefr', this.profilerInput);
    this.profilerService.downloadFile(request).subscribe(
      success => {
        console.log(success);
        const blob = new Blob([success], { type: 'application/msword' });
        var downloadURL = window.URL.createObjectURL(success);
        var link = document.createElement('a');
        link.href = downloadURL;
        link.download = "vocab-profile-"+ this.getFormattedDate() + ".doc";
        link.click();
      },
      err => {
        alert("Server error while downloading file.");
      }
    );
    this.isCefr = true;

  }

  getFormattedDate() {
    var date = new Date();
    var str = date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate() + "-" + date.getHours() + "-" + date.getMinutes() + "-" + date.getSeconds();
    return str;
  }

  submitAwl() {
    const request = new ProfilerRequest('awl', this.profilerInput);
    this.submitToProfiler(request);
    this.isCefr = false;
    this.isNawl = false;
    this.scrollToTop();
  }

  submitNawl() {
    const request = new ProfilerRequest('nawl', this.profilerInput);
    this.submitToProfiler(request);
    this.isCefr = false;
    this.isNawl = true;
    this.scrollToTop();
  }

  scrollToTop() {
    window.scrollTo({
      top: 0,
      behavior: 'smooth',
    });
  }

  closeSignInAlert() {
    this.hasSignInAlert = false;
  }

  saveReading() {
    this.readingService.postFromProfiler(this.profilerInput).subscribe(r => {
      this.router.navigate(['/org/details', r.orgId, 'reading', 'details', r.id]);
    });
  }

  private submitToProfiler(request: ProfilerRequest) {
    this.profilerService.profile(request).subscribe(
      data => { this.profileResult = data; },
      err => console.error(err)
    );
  }

  toggleColor(event, cssClass) {
    if (event.srcElement.classList.contains('btn-outline-secondary')) {
      this.renderer.removeClass(event.srcElement, 'btn-outline-secondary');
      this.renderer.addClass(event.srcElement, 'btn-secondary-lite');
    } else {
      this.renderer.addClass(event.srcElement, 'btn-outline-secondary');
      this.renderer.removeClass(event.srcElement, 'btn-secondary-lite');
    }

    let elements = this.elem.nativeElement.querySelector("#profileResult").querySelectorAll(cssClass);
    const offList = "profilerOffList";
    elements.forEach(e => {
      if (e.classList.contains(offList)) {
        this.renderer.removeClass(e, offList);
        let btn = this.elem.nativeElement.querySelector("profileResult");
      } else {
        this.renderer.addClass(e, offList);
      }
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }


}

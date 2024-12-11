import { Component, OnInit } from '@angular/core';
import { AuthService } from '@services/auth.service';
import { Subscription } from 'rxjs/Subscription';
import { Router } from '@angular/router';
import { AccessService } from '@services/access.service';
import { IUserAccess } from '@models/user-access.interface';
import { tap, mergeMap } from 'rxjs/operators';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isCollapsed = true;
  isLoggedIn: boolean;
  access: IUserAccess;
  authSub: Subscription;
  accessSub: Subscription;

  constructor(
    private userService: AuthService,
    private router: Router,
    private accessService: AccessService,
  ) { }

  ngOnInit() {
    this.authSub = this.userService.authNavStatus$.pipe(
      tap(
        loggedIn => {
          if (loggedIn)
            this.accessSub = this.accessService.get().subscribe(result => {
              this.access = result;

              if (!this.access.isAdmin && !this.access.isTeacher && !this.access.isStudent) {
                this.userService.logout();
              }

            });
        })).subscribe(result => this.isLoggedIn = result);

    this.router.events.subscribe(e => { this.isCollapsed = true; });
  }

  signOut() {
    this.userService.logout();
    this.router.navigate(['/home']);
  }

  get isAdmin(): boolean {
    if (!this.access)
      return false;

    return this.access.isAdmin;
  }

  get isTeacher(): boolean {
    if (!this.access)
      return false;

    return this.access.isTeacher;
  }

  get isStudent(): boolean {
    if (!this.access)
      return false;

    return this.access.isStudent;
  }

  ngOnDestroy() {
    this.authSub.unsubscribe();
    this.accessSub.unsubscribe();
  }
}

import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { HammerModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

// Ignite UI for Angular Components
import { IgxNavigationDrawerComponent, IgxNavigationDrawerModule } from 'igniteui-angular';
import { IgxNavbarComponent } from 'igniteui-angular';
import { IgxButtonDirective } from 'igniteui-angular';
import { IgxButtonModule } from 'igniteui-angular';
import { IgxRippleModule } from 'igniteui-angular';
import { IgxAvatarModule } from 'igniteui-angular';
import { IgxInputGroupModule } from 'igniteui-angular';
import { IgxIconModule } from 'igniteui-angular';
import { Router } from '@angular/router';
import { AppConfigService } from '../../services/configurationService';
import { AuthenticationService } from '../../services/authenticationService';
@Component({
  selector: 'app-applicationheader',
  templateUrl: './applicationheader.component.html',
  styleUrl: './applicationheader.component.css',
  standalone: true,
  imports: [
    BrowserModule,
    FormsModule,
    MatToolbarModule,
    MatIconModule,
    MatChipsModule,
    BrowserAnimationsModule, HammerModule, IgxNavigationDrawerModule,
    IgxButtonModule, IgxRippleModule, IgxAvatarModule, IgxInputGroupModule, IgxIconModule
  ]

})
export class ApplicationheaderComponent {

  @Output() menuClick = new EventEmitter<void>();
  @Output() logoutClick = new EventEmitter<void>();

  onMenuClick(): void {
    this.menuClick.emit();
  }

  constructor(private router: Router,
    private appConfigService: AppConfigService, public authenticationService: AuthenticationService
  ) {}

  ngOnInit() {}

  ngOnDestroy() {}

  onIconClick() {
    // Move to the /register route
    this.router.navigate(['/register']);
  }

  onAdminPanelClick() {
    // Move to the /admin route
    this.router.navigate(['/adminpanel']);
  }
  onImageClick() {
    // Move to the /login route
    this.router.navigate(['/']);
  }
  onLogoutClick() {
    // Log out the user
    this.authenticationService.logout();
    // Move to the /login route
    this.router.navigate(['/']);
    this.logoutClick.emit();
  }

  isUserAdmin(): boolean {
    return this.authenticationService.getFirstUserRole() === 'Admin';
  }
}

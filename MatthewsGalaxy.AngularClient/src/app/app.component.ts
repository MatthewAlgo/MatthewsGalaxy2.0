import { AfterViewInit, Component, ElementRef, HostListener, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { IgxNavigationDrawerComponent } from 'igniteui-angular';
import { Router } from '@angular/router';
import { AppConfigService } from '../services/configurationService';
import { AuthenticationService } from '../services/authenticationService';
import { LoginviewcomponentComponent } from './loginviewcomponent/loginviewcomponent.component';
import { HomeService } from '../services/homeService';
import { IdentityUser } from '../models/user';
import { UserService } from '../services/userService';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit, AfterViewInit, OnDestroy {

  @ViewChild(IgxNavigationDrawerComponent, { static: true })
  public drawer!: IgxNavigationDrawerComponent;

  public selected;
  public navItems: { name: string, text: string, path: string }[] = [];

  constructor(
    private http: HttpClient,
    private appConfigService: AppConfigService,
    private authenticationService: AuthenticationService,
    private userService: UserService,
    private router: Router
  ) {
    this.navItems = [
      { name: 'account_circle', text: 'Home Page', path: '/' },
      { name: 'error', text: 'About Me', path: '/about-me' },
      { name: 'group_work', text: 'Categories', path: '/categories' },
      { name: 'folder', text: 'My Projects', path: '/my-projects' },
    ];
    this.selected = this.navItems[0].text;

    if (this.authenticationService.getToken() !== null) {
      this.navItems.push({ name: 'logout', text: 'Logout', path: '/logout' });

      this.userService.getCurrentUser().subscribe((user: IdentityUser) => {
        if (user.roles && user.roles.length > 0 && user.roles[0] === 'Guest') {
          this.navItems.push({ name: 'account_circle', text: 'Become a member', path: '/become-a-member' });
        }
      });
    } else {
      this.navItems.push({ name: 'login', text: 'Login', path: '/login' });
    }
  }

  public navigate(item: { name: string, text: string, path: string }): void {
    if (item.text === 'Logout') {
      this.authenticationService.logout();
      this.router.navigate(['/']).then(() => {
        this.navItems = [
          { name: 'account_circle', text: 'Home Page', path: '/' },
          { name: 'error', text: 'About Me', path: '/about-me' },
          { name: 'group_work', text: 'Categories', path: '/categories' },
          { name: 'folder', text: 'My Projects', path: '/my-projects' },
        ];
        this.navItems.push({ name: 'login', text: 'Login', path: '/login' });
      });
      this.drawer.close();
    } else {
      this.selected = item.text;
      this.router.navigate([item.path]);
      this.drawer.close();
    }
  }

  onActivate(componentRef: any) {
    if (componentRef instanceof LoginviewcomponentComponent) {
      componentRef.loginSuccess.subscribe(() => this.handleLoginSuccess());
    }
  }

  ngOnInit() {}

  handleMenuClick() {
    this.drawer.toggle();
  }

  handleLogoutClick() {
    this.navItems = [
      { name: 'account_circle', text: 'Home Page', path: '/' },
      { name: 'error', text: 'About Me', path: '/about-me' },
      { name: 'group_work', text: 'Categories', path: '/categories' },
      { name: 'folder', text: 'My Projects', path: '/my-projects' },
    ];
    this.navItems.push({ name: 'login', text: 'Login', path: '/login' });
  }

  handleLoginSuccess() {
    this.navItems = [
      { name: 'account_circle', text: 'Home Page', path: '/' },
      { name: 'error', text: 'About Me', path: '/about-me' },
      { name: 'group_work', text: 'Categories', path: '/categories' },
      { name: 'folder', text: 'My Projects', path: '/my-projects' },
    ];
    this.navItems.push({ name: 'logout', text: 'Logout', path: '/logout' });

    this.userService.getCurrentUser().subscribe((user: IdentityUser) => {
      if (user.roles && user.roles.length > 0 && user.roles[0] === 'Guest') {
        this.navItems.push({ name: 'account_circle', text: 'Become a member', path: '/become-a-member' });
      }
    });
  }

  @ViewChild('navigation') navigation!: ElementRef;
  @ViewChild('overlay') overlay!: ElementRef;

  private drawerOpeningListener!: () => void;
  private drawerClosingListener!: () => void;
  private overlayClickListener!: () => void;

  ngAfterViewInit(): void {
    if (this.navigation && this.overlay) {
      this.drawerOpeningListener = () => this.overlay.nativeElement.classList.add('active');
      this.drawerClosingListener = () => this.overlay.nativeElement.classList.remove('active');
      this.overlayClickListener = () => this.drawer.close(); // Close the drawer when the overlay is clicked

      this.navigation.nativeElement.addEventListener('igxDrawerOpening', this.drawerOpeningListener);
      this.navigation.nativeElement.addEventListener('igxDrawerClosing', this.drawerClosingListener);
      this.overlay.nativeElement.addEventListener('click', this.overlayClickListener); // Add click event listener to the overlay
    }
  }

  ngOnDestroy(): void {
    if (this.navigation && this.overlay) {
      this.navigation.nativeElement.removeEventListener('igxDrawerOpening', this.drawerOpeningListener);
      this.navigation.nativeElement.removeEventListener('igxDrawerClosing', this.drawerClosingListener);
      this.overlay.nativeElement.removeEventListener('click', this.overlayClickListener); // Remove the click event listener
    }
  }

  @HostListener('document:mousemove', ['$event'])
  onMouseMove(event: MouseEvent): void {
    const imageWrapper = document.querySelector('.image_wrapper') as HTMLElement;

    if (imageWrapper) {
      const { clientX, clientY } = event;
      const { innerWidth, innerHeight } = window;

      const xPos = (clientX / innerWidth) * 10; // Adjust 10 for the intensity of movement
      const yPos = (clientY / innerHeight) * 10; // Adjust 10 for the intensity of movement

      imageWrapper.style.backgroundPosition = `${50 + xPos}% ${50 + yPos}%`;
    }
  }

  title = 'Matthew\'s Galaxy';
}

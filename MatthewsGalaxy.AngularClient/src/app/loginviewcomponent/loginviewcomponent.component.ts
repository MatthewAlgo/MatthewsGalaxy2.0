import { Component, EventEmitter, Inject, Output } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../services/authenticationService';  // Adjust the path as necessary
import { AppConfigService } from '../../services/configurationService';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-loginviewcomponent',
  templateUrl: './loginviewcomponent.component.html',
  styleUrls: ['./loginviewcomponent.component.css'],  // Fixed typo from 'styleUrl' to 'styleUrls'
})
export class LoginviewcomponentComponent {
  @Output() loginSuccess = new EventEmitter<void>();

  constructor(private authService: AuthenticationService, private router: Router,
      private appConfigService: AppConfigService, private snackBar: MatSnackBar
    ) {}

  ngOnInit() {}

  ngOnDestroy() {}

  onSubmit(loginForm: any) {
    if (loginForm.valid) {
      const { username, password } = loginForm.value;
      this.authService.login(username, password).subscribe((response) => {
        this.authService.setToken(response.token);
        this.router.navigate(['/']).then(() => {
          this.snackBar.open('Login successful!', 'Close', { duration: 3000 });
        });
        this.loginSuccess.emit();
      }, (error) => {
        this.snackBar.open('Login failed! - ' + error.error, 'Close', { duration: 3000 });
        console.error('Login failed!', error);
      });
    }
  }
}

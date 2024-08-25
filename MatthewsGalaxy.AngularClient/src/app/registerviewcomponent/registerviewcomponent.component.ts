import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthenticationService } from '../../services/authenticationService';
import { AppConfigService } from '../../services/configurationService';
import { ReCaptchaV3Service } from 'ng-recaptcha';
import { environment } from '../../env/environment';

@Component({
  selector: 'app-registerviewcomponent',
  templateUrl: './registerviewcomponent.component.html',
  styleUrls: ['./registerviewcomponent.component.css'],
})
export class RegisterviewcomponentComponent {
  reCAPTCHAToken: string | undefined;

  constructor(
    private router: Router,
    private http: HttpClient,
    private snackBar: MatSnackBar,
    private authenticationService: AuthenticationService,
    private appConfigService: AppConfigService,
  ) {}

  onCaptchaResolved(token: string | null) {
    if (token) {
      this.reCAPTCHAToken = token;
    }
  }

  onSubmit(form: NgForm) {
    if (form.value.password !== form.value.confirmPassword) {
      this.snackBar.open('Passwords do not match!', 'Close', { duration: 3000 });
      return;
    }

    const passwordRegex = /^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])/;
    if (!passwordRegex.test(form.value.password)) {
      this.snackBar.open('Passwords must have at least one non alphanumeric character, one digit, and one uppercase character.', 'Close', { duration: 3000 });
      return;
    }

    if (!this.reCAPTCHAToken) {
      this.snackBar.open('Please complete the reCAPTCHA.', 'Close', { duration: 3000 });
      return;
    }

    if (form.valid) {
      const { username, email, password } = form.value;
      const apiUrl = `${this.appConfigService.API_SERVER_IP_ADDRESS}/auth/User/register`;

      this.authenticationService.register(username, email, password).subscribe(
        (response) => {
          this.snackBar.open('Registration successful!', 'Close', { duration: 3000 });
          this.router.navigate(['/login']).then(() => {
            this.snackBar.open('Please log in with your new account.', 'Close', { duration: 3000 });
          });
        },
        (error) => {
          this.snackBar.open('Registration failed!', 'Close', { duration: 3000 });
          console.error('Registration failed!', error);
        }
      );
    } else {
      this.snackBar.open('Form is invalid!', 'Close', { duration: 3000 });
      for (const control of Object.keys(form.controls)) {
        form.controls[control].markAsTouched();
      }
      return;
    }
  }
}

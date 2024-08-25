import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../services/authenticationService';
import { AppConfigService } from '../../services/configurationService';
import { EmailNewsletterService } from '../../services/emailNewsletterService';

@Component({
  selector: 'app-applicationfooter',
  templateUrl: './applicationfooter.component.html',
  styleUrls: ['./applicationfooter.component.css'],
})
export class ApplicationfooterComponent {
  reCAPTCHAToken: string | undefined;

  constructor(
    private router: Router,
    private http: HttpClient,
    private snackBar: MatSnackBar,
    private authenticationService: AuthenticationService,
    private appConfigService: AppConfigService,
    private emailNewsletterService: EmailNewsletterService
  ) {}

  onCaptchaResolved(token: string | null) {
    this.reCAPTCHAToken = token || undefined;
  }

  onSubmit(form: NgForm) {
    if (!this.reCAPTCHAToken) {
      this.snackBar.open('Please complete the reCAPTCHA.', 'Close', { duration: 3000 });
      return;
    }

    if (form.valid) {
      const { email } = form.value;

      this.emailNewsletterService.registerNewsletterEmail(email).subscribe(
        () => {
          form.reset();
        },
        (error) => {
          console.error('Email registration failed!', error);
        }
      );
    }
  }
}

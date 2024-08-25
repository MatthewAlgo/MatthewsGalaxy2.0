import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { AppConfigService } from './configurationService';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class EmailNewsletterService {

  private apiUrl = `${this.appConfigService.API_SERVER_IP_ADDRESS}/api/EmailVerification`;
  private LoggedInUserName?: string;
  private LoggedInUserEmail?: string;
  private LoggedInUserRoles?: string[];
  private JWTAuthenticationToken?: string;

  constructor(
    private http: HttpClient,
    private appConfigService: AppConfigService,
    private snackBar: MatSnackBar,
    private router: Router
  ) { }

  registerNewsletterEmail(email: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/add-email-address-subscriptions/${email}`, null).pipe(
      tap(response => {
        if (response) {
          this.snackBar.open('Email registration successful! Please verify your email.', 'Close', { duration: 3000 });
        }
      }),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 200) {
          this.snackBar.open('Email registration successful! Please verify your email.', 'Close', { duration: 3000 });
          return throwError(() => new Error('Email registration acknowledged with JSON parsing issues.'));
        }
        let errorMessage = 'Registration failed!';
        if (error.error instanceof ErrorEvent) {
          // Client-side or network error
          errorMessage = `Client error: ${error.error.message}`;
        } else {
          // Server-side error
          if (error.error && typeof error.error === 'string') {
            // If the error response is a string
            errorMessage = `Server error: ${error.error}`;
          } else {
            // Default message for non-string errors
            errorMessage = `Error ${error.status}: ${error.statusText}`;
          }
        }
        this.snackBar.open(errorMessage, 'Close', { duration: 3000 });
        console.error(errorMessage, error);
        return throwError(() => new Error(errorMessage));
      })
    );
  }
}

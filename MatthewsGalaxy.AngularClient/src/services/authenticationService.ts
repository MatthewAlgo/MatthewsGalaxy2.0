import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { AppConfigService } from './configurationService';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private apiUrl = this.appConfigService.API_SERVER_IP_ADDRESS + '/auth/User'; // Replace with your API URL
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

  login(username: string, password: string): Observable<any> {
    return this.http.post<TokenResponse>(`${this.apiUrl}/login`, { username, password }).pipe(
      tap((response: TokenResponse) => {
        this.setToken(response.token);
        this.setUserName(response.userName);
        this.setEmail(response.userEmail);
        this.setFirstUserRole(response.userRole);
        this.router.navigate(['/']).then(() => {
          this.snackBar.open('Login successful!', 'Close', { duration: 3000 });
        });
      }),
      catchError((error) => {
        this.snackBar.open('Login failed!', 'Close', { duration: 3000 });
        console.error('Login failed!', error);
        return throwError(error);
      })
    );
  }

  register(username: string, email: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, { username, email, password }).pipe(
      tap(() => {
        this.snackBar.open('Registration successful!', 'Close', { duration: 3000 });
        this.router.navigate(['/login']).then(() => {
          this.snackBar.open('Please log in with your new account.', 'Close', { duration: 3000 });
        });
      }),
      catchError((error) => {
        this.snackBar.open('Registration failed!', 'Close', { duration: 3000 });
        console.error('Registration failed!', error);
        return throwError(error);
      })
    );
  }

  setToken(token: string): void {
    localStorage.setItem('authToken', token);
  }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  getFirstUserRole(): string | null {
    return localStorage.getItem('userRole');
  }

  setFirstUserRole(userRole: string): void {
    localStorage.setItem('userRole', userRole);
  }

  logout(): void {
    this.JWTAuthenticationToken = undefined;
    this.LoggedInUserName = undefined;
    this.LoggedInUserEmail = undefined;
    localStorage.removeItem('authToken');
    localStorage.removeItem('userName');
    localStorage.removeItem('userEmail');
    localStorage.removeItem('userRole');
    this.router.navigate(['/']).then(() => {
      this.snackBar.open('Logout successful!', 'Close', { duration: 3000 });
    });
  }

  getUsername(): string | null {
    return localStorage.getItem('userName');
  }

  setUserName(username: string): void {
    localStorage.setItem('userName', username);
  }

  getEmail(): string | null {
    return localStorage.getItem('userEmail');
  }

  setEmail(email: string): void {
    localStorage.setItem('userEmail', email);
  }

  getCurrentUser() {
    return this.http.get(`${this.apiUrl}/currentuser`);
  }

}

interface TokenResponse {
  token: string;
  userName: string;
  userEmail: string;
  userRole: string;
}

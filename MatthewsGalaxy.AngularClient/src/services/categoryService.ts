import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Category } from '../models/category';
import { AppConfigService } from './configurationService';

@Injectable({
  providedIn: 'root',
})

export class CategoryService {
  private apiUrl = this.appConfigService.API_SERVER_IP_ADDRESS + '/api/Categories'; // Replace with your API URL

  constructor(private http: HttpClient, private appConfigService
    : AppConfigService
  ) {}

  // Method to get all categories
  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.apiUrl).pipe(
      catchError(this.handleError<Category[]>('getCategories', []))
    );
  }

  // Error handling method
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed: ${error.message}`);
      return of(result as T);
    };
  }
}

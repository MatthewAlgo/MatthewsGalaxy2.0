import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppConfigService } from './configurationService';
import { Comment } from '../models/comment';

@Injectable({
  providedIn: 'root'
})
export class CommentsService {

  private apiUrl = this.appConfigService.API_SERVER_IP_ADDRESS + '/api/Comments'; // Replace with your API URL

  constructor(private http: HttpClient
    , private appConfigService: AppConfigService
  ) { }

  getAllComments(): Observable<any> {
    return this.http.get(this.apiUrl);
  }

  getCommentById(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`);
  }

  getNumberOfComments(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/count`);
  }

  getCommentsForArticle(articleId: string): Observable<Comment[]>
  {
    return this.http.get<Comment[]>(`${this.apiUrl}/article/${articleId}`);
  }

  addComment(articleId: string, commentContent: string, commentTitle: string, username: string, useremail: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/add/${articleId}/${username}/${useremail}`, {
      Title: commentTitle,
      Content: commentContent
    });
  }

  deleteComment(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete/${id}`);
  }
}

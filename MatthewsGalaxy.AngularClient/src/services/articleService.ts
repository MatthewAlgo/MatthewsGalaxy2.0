import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin, map, Observable, switchMap } from 'rxjs';
import { Article } from '../models/article';
import { AppConfigService } from './configurationService';
import { Category } from '../models/category';
import { Tag } from '../models/tag';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {
  private apiUrl = this.appConfigService.API_SERVER_IP_ADDRESS + '/api/BlogPosts'; // Replace with your API URL

  constructor(private http: HttpClient,
    private appConfigService : AppConfigService
  ) { }

  getAllArticles(): Observable<Article[]> {
    return this.http.get<Article[]>(this.apiUrl);
  }

  // Get all articles and tags
  getAllArticlesAndTags(): Observable<Article[]> {
      return this.getAllArticles().pipe(
          switchMap((data: Article[]) => {
              // For each article, get the categories and tags
              const categoryRequests = data.map(article =>
                  this.getCategoriesOfArticle(article.computedHashId!).pipe(
                      map((categories: Category[]) => {
                          article.categories = categories.map(category => category.name!);
                      })
                  )
              );
              const tagRequests = data.map(article =>
                  this.getTagsOfArticle(article.computedHashId!).pipe(
                      map((tags: Tag[]) => {
                          article.tags = tags.map(tag => tag.tagName!);
                      })
                  )
              );
              return forkJoin([...categoryRequests, ...tagRequests]).pipe(
                  map(() => data)
              );
          })
      );
  }

  getArticleById(id: string): Observable<Article> {
    return this.http.get<Article>(`${this.apiUrl}/${id}`);
  }

  getNumberOfArticles(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/count`);
  }

  getNumberOfLikesOfArticle(id: string): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/${id}/likes`);
  }

  likeArticle(id: string, userName: string, userEmail: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/like/${id}/${userName}/${userEmail}`, {});
  }

  unlikeArticle(id: string, userName: string, userEmail: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/unlike/${id}/${userName}/${userEmail}`, {});
  }

  isLikedByUser(id: string, username: string, email: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/isliked/${id}/${username}/${email}`);
  }

  removeArticle(computedHashId: any) : Observable<any> {
    return this.http.delete(`${this.apiUrl}/${computedHashId}`);
  }
  addArticle(localArticle: { title: string, description: string, shortDescription: string, featuredImageURL: string }) {
    return this.http.post(this.apiUrl, localArticle);
  }
  updateArticle(localArticle: Article) {
    throw new Error('Method not implemented.');
  }

  incrementArticleViews(computedHashId: string | undefined) {
    return this.http.post(`${this.apiUrl}/incrementViews/${computedHashId}`, {});
  }

  getCategoriesOfArticle(computedHashId: string) : Observable<Category[]> {
    return this.http.get<Category[]>(`${this.apiUrl}/${computedHashId}/categories`);
  }
  getTagsOfArticle(computedHashId: string) : Observable<Tag[]> {
    return this.http.get<Tag[]>(`${this.apiUrl}/${computedHashId}/tags`);
  }

  addCategoryToArticle(computedHashId: string, category: string) {
    return this.http.post(`${this.apiUrl}/${computedHashId}/categories/${category}`, {});
  }

  addTagToArticle(computedHashId: string, tag: string) {
    return this.http.post(`${this.apiUrl}/${computedHashId}/tags/${tag}`, {});
  }

  removeCategoryFromArticle(computedHashId: string, category: string) {
    return this.http.delete(`${this.apiUrl}/${computedHashId}/categories/${category}`);
  }

  removeTagFromArticle(computedHashId: string, tag: string) {
    return this.http.delete(`${this.apiUrl}/${computedHashId}/tags/${tag}`);
  }

  getArticlesByCategoryId(id: string | undefined) : Observable<Article[]> {
    return this.http.get<Article[]>(`${this.apiUrl}/categoryid/${id}`);
  }

  getArticlesByCategoryName(name: string | undefined) : Observable<Article[]> {
    return this.http.get<Article[]>(`${this.apiUrl}/categoryname/${name}`);
  }

}

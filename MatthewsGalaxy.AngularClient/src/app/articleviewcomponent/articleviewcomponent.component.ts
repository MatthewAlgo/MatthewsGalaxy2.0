import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ArticleService } from '../../services/articleService';
import { NgxMasonryModule } from 'ngx-masonry';
import { Article } from '../../models/article';
import { ArticlecardComponent } from '../articlecard/articlecard.component';
import { NgFor, NgClass, NgIf } from '@angular/common';

@Component({
  selector: 'app-articleviewcomponent',
  templateUrl: './articleviewcomponent.component.html',
  styleUrls: ['./articleviewcomponent.component.css'],
  standalone: true,
  imports: [
    NgxMasonryModule,
    ArticlecardComponent,
    NgFor,
    NgClass,
    NgIf
  ],
})
export class ArticleviewcomponentComponent implements OnInit {
  articles: Article[] = [];

  constructor(private http: HttpClient, private articleService: ArticleService) {}

  ngOnInit() {
    this.articleService.getAllArticles().subscribe((data: Article[]) => {
      this.articles = data;
    });
  }
}

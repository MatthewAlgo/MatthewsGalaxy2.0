import { HttpClient } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { ArticleService } from '../../services/articleService';
import { Article } from '../../models/article';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-categoriesforarticlescomponent',
  templateUrl: './categoriesforarticlescomponent.component.html',
  styleUrls: ['./categoriesforarticlescomponent.component.css']
})
export class CategoriesforarticlescomponentComponent {
  @Input() articles?: Article[];
  categoryName: string | null = null;

  constructor(
    private http: HttpClient,
    private articleService: ArticleService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    // Check if articles are provided as @Input, if not fetch based on categoryName
    if (!this.articles) {
      this.route.paramMap.subscribe(params => {
        this.categoryName = params.get('categoryName');
        if (this.categoryName) {
          this.articleService.getArticlesByCategoryName(this.categoryName).subscribe((data: Article[]) => {
            this.articles = data;
          });
        }
      });
    }
  }
}

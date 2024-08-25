import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ArticleService } from '../../services/articleService';
import { CategoryService } from '../../services/categoryService';
import { Category } from '../../models/category';
import { Article } from '../../models/article';

@Component({
  selector: 'app-categoriesviewcomponent',
  templateUrl: './categoriesviewcomponent.component.html',
  styleUrl: './categoriesviewcomponent.component.css'
})
export class CategoriesviewcomponentComponent implements OnInit {
  constructor(private http: HttpClient, private articleService: ArticleService, private categoryService: CategoryService) {}

  categories: Category[] = [];
  noCategoriesShown: boolean = false;

  ngOnInit() {
    this.categoryService.getCategories().subscribe((data: any) => {
      this.categories = data;
      // If the number of categories is 0, show a message
      if (this.categories.length === 0) {
        this.noCategoriesShown = true;
      }
      // For each category, get the number of articles in that category
      for (let i = 0; i < this.categories.length; i++) {
        this.articleService.getArticlesByCategoryName(this.categories[i].name).subscribe((data: Article[]) => {
          this.categories[i]!.numberOfArticles = data.length;
          this.categories[i]!.correspondingArticles = data;
        });
      }
    });


  }

}

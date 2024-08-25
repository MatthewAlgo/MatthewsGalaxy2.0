import { ChangeDetectorRef, Component, NgZone, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { ArticleService } from '../../services/articleService';
import { UserService } from '../../services/userService';
import { ProjectService } from '../../services/projectService';
import { Article } from '../../models/article';
import { IdentityUser } from '../../models/user';
import { Project } from '../../models/project';
import { Category } from '../../models/category';
import { Tag } from '../../models/tag';

@Component({
  selector: 'app-adminpanelcomponent',
  templateUrl: './adminpanelcomponent.component.html',
  styleUrls: ['./adminpanelcomponent.component.css']
})
export class AdminpanelcomponentComponent implements OnInit {

  currentUserRole: string | undefined;
  articles: Article[] = [];
  users: IdentityUser[] = [];
  projects: Project[] = [];

  constructor(
    private http: HttpClient,
    private articleService: ArticleService,
    private userService: UserService,
    private router: Router,
    private snackbar: MatSnackBar,
    private projectService: ProjectService, private zone: NgZone,
  ) {}

  ngOnInit() {
    this.articleService.getAllArticlesAndTags().subscribe((data: Article[]) => {
      this.articles = data;
    });
    this.userService.getAllUsers().subscribe((data: IdentityUser[]) => {
      this.users = data;
    });
    this.projectService.getProjects().subscribe((data: Project[]) => {
      this.projects = data;
    });
    this.userService.getCurrentUser().subscribe((data: IdentityUser) => {
      if (data.roles && data.roles.length > 0) {
        this.currentUserRole = data.roles[0];
      }
    });
  }

  createArticle() {
    this.router.navigate(['/create-article']);
  }

  createProject() {
    this.router.navigate(['/create-project']);
  }

  removeArticle(article: Article) {
    if (confirm('Are you sure you want to delete this article?')) {
      this.articleService.removeArticle(article.computedHashId).subscribe(() => {
        this.articles = this.articles.filter(a => a.computedHashId !== article.computedHashId);
      });
    }
  }

  removeProject(project: Project) {
    if (confirm('Are you sure you want to delete this project?')) {
      this.projectService.deleteProject(project.id!).subscribe(
        () => {
          this.projects = this.projects.filter(p => p.id !== project.id);
          this.snackbar.open('Project deleted successfully', 'Close', {
            duration: 5000,
          });
        },
        (error) => {
          this.snackbar.open('Error: ' + error.error, 'Close', {
            duration: 5000,
          });
        }
      );
    }
  }

  removeUser(user: IdentityUser) {
    if (confirm('Are you sure you want to delete this user?')) {
      this.userService.removeUser(user.id).subscribe(
        () => {
          this.users = this.users.filter(u => u.id !== user.id);
          this.snackbar.open('User deleted successfully', 'Close', {
            duration: 5000,
          });
        },
        (error) => {
          this.snackbar.open('Error: ' + error.error, 'Close', {
            duration: 5000,
          });
        }
      );
    }
  }

  addTag(article: Article) {
    const tagName = prompt('Enter the tag name:');
    if (tagName) {
      this.articleService.addTagToArticle(article.computedHashId!, tagName).subscribe(() => {
        if (article.tags) {
          article.tags.push(tagName);
        }
        this.snackbar.open('Tag added successfully.', 'Close', { duration: 3000 });

        this.articleService.getTagsOfArticle(article.computedHashId!).subscribe((data: Tag[]) => {
          article.tags = data.map(tag => tag.tagName!);
          // Update the whole articles array to reflect the changes

          this.articleService.getAllArticlesAndTags().subscribe((data: Article[]) => {
            this.articles = data;
          });
        });
      });
    }
  }

  removeTag(article: Article, tag: string) {
    if (confirm(`Are you sure you want to remove the tag "${tag}"?`)) {
      this.articleService.removeTagFromArticle(article.computedHashId!, tag).subscribe(() => {
        article.tags = article.tags!.filter(t => t !== tag);
        this.snackbar.open('Tag removed successfully.', 'Close', { duration: 3000 });
        // Update the whole articles array to reflect the changes
        this.articleService.getAllArticlesAndTags().subscribe((data: Article[]) => {
          this.articles = data;
        });
      });
    }
  }


  addCategory(article: Article) {
    const categoryName = prompt('Enter the category name:');
    if (categoryName) {
      this.articleService.addCategoryToArticle(article.computedHashId!, categoryName).subscribe(() => {
        if (article.categories) {
          article.categories.push(categoryName);
        }
        this.snackbar.open('Category added successfully.', 'Close', { duration: 3000 });

        this.articleService.getCategoriesOfArticle(article.computedHashId!).subscribe((data: Category[]) => {
          article.categories = data.map(category => category.name!);
          // Update the whole articles array to reflect the changes
          this.articleService.getAllArticlesAndTags().subscribe((data: Article[]) => {
            this.articles = data;
          });
        });
      });
    }
  }

  removeCategory(article: Article, category: string) {
    if (confirm(`Are you sure you want to remove the category "${category}"?`)) {
      this.articleService.removeCategoryFromArticle(article.computedHashId!, category).subscribe(() => {
        article.categories = article.categories!.filter(cat => cat !== category);
        this.snackbar.open('Category removed successfully.', 'Close', { duration: 3000 });
        // Update the whole articles array to reflect the changes
        this.articleService.getAllArticlesAndTags().subscribe((data: Article[]) => {
          this.articles = data;
        });
      });
    }
  }

  getColorForString(str: string): string {
    let hash = 0;
    for (let i = 0; i < str.length; i++) {
      hash = str.charCodeAt(i) + ((hash << 5) - hash);
    }
    const color = `hsl(${hash % 360}, 70%, 70%)`;
    return color;
  }
}

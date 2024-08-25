import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommentsService } from '../../services/commentsService';
import { ArticleService } from '../../services/articleService';
import { Article } from '../../models/article';
import { Comment } from '../../models/comment';
import { Category } from '../../models/category';
import { Tag } from '../../models/tag';
import { AuthenticationService } from '../../services/authenticationService';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-viewarticlewithidcomponent',
  templateUrl: './viewarticlewithidcomponent.component.html',
  styleUrls: ['./viewarticlewithidcomponent.component.css']
})
export class ViewarticlewithidcomponentComponent implements OnInit {

  articleId!: string;
  comments: Comment[] | undefined;
  article: Article | undefined;
  categories: Category[] = [];
  tags: Tag[] = [];
  commentTitle: string = '';
  commentBody: string = '';

  constructor(
    private route: ActivatedRoute,
    private articleService: ArticleService,
    private commentService: CommentsService,
    private authenticationService: AuthenticationService,
    private snackBar: MatSnackBar,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.articleId = params['id'];
      this.articleService.getArticleById(this.articleId).subscribe(article => {
        this.article = article;
        this.loadCategoriesAndTags();
      });
      this.loadComments();
    });
  }

  loadCategoriesAndTags() {
    if (this.article?.computedHashId) {
      this.articleService.getCategoriesOfArticle(this.article.computedHashId).subscribe((categories: Category[]) => {
        this.categories = categories;
      });
      this.articleService.getTagsOfArticle(this.article.computedHashId).subscribe((tags: Tag[]) => {
        this.tags = tags;
      });
    }
  }

  getTagColor(tag: Tag): string {
    let hash = 0;
    for (let i = 0; i < tag.tagName!.length; i++) {
      hash = tag.tagName!.charCodeAt(i) + ((hash << 5) - hash);
    }
    const color = `hsl(${hash % 360}, 70%, 70%)`;
    return color;
  }

  getCategoryColor(category: Category): string {
    let hash = 0;
    for (let i = 0; i < category.name!.length; i++) {
      hash = category.name!.charCodeAt(i) + ((hash << 5) - hash);
    }
    const color = `hsl(${hash % 360}, 70%, 70%)`;
    return color;
  }

  redirectToCategory(_t17: Category) {
    this.router.navigate(
      ['/category', _t17.name]
    );
  }

  submitComment() {
    if (this.commentTitle.length > 100) {
      this.showErrorSnackbar('Comment title should not exceed 100 characters.');
      return;
    }
    if (this.commentBody.length > 500) {
      this.showErrorSnackbar('Comment body should not exceed 500 characters.');
      return;
    }

    if (this.commentTitle.length === 0 || this.commentBody.length === 0) {
      this.showErrorSnackbar('Comment title and body are required.');
      return;
    }

    // If the user is not logged in, show an error message, and redirect to the login page
    if (!this.authenticationService.isLoggedIn()) {
      this.showErrorSnackbar('You must be logged in to comment.');
      this.router.navigate(['/login']);
      return;
    }

    this.saveComment(this.commentTitle, this.commentBody);
  }

  removeComment(comment: Comment) {
    const confirmation = window.confirm('Are you sure you want to delete this comment?');

    if (confirmation) {
      this.commentService.deleteComment(comment.id).subscribe(() => {
        this.loadComments();
        // Show success snackbar
        this.snackBar.open('Comment deleted successfully.', 'Close', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'bottom',
          panelClass: ['success-snackbar']
        });
      });
    }
  }

  saveComment(title: string, body: string) {
    this.commentService.addComment(this.articleId, body, title, this.authenticationService.getUsername()!, this.authenticationService.getEmail()!).subscribe(
      () => {
        this.loadComments();
        this.commentTitle = '';
        this.commentBody = '';
        this.snackBar.open('Comment saved successfully.', 'Close', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'bottom',
          panelClass: ['success-snackbar']
        });
      },
      error => {
        if (error.status === 400) {
          if (error.error && error.error.GetContent && error.error.GetContent.includes("You have already commented on this article.")) {
            this.showErrorSnackbar('You have already commented on this article.');
          } else {
            this.showErrorSnackbar('Failed to save comment. Please try again later.');
          }
        } else {
          this.showErrorSnackbar('Failed to save comment. Please try again later.');
        }
      }
    );
  }

  loadComments() {
    this.commentService.getCommentsForArticle(this.articleId).subscribe(comments => {
      this.comments = comments;
    });
  }

  showErrorSnackbar(message: string) {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: ['error-snackbar']
    });
  }
}

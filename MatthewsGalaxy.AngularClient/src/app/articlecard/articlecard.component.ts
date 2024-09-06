import { MatChip } from '@angular/material/chips';
import { ChangeDetectionStrategy, EventEmitter, NgModule, Output, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Article } from '../../models/article';
import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { ArticleService } from '../../services/articleService';
import { AuthenticationService } from '../../services/authenticationService';
import { HttpClient } from '@angular/common/http';
import { Category } from '../../models/category';
import { Tag } from '../../models/tag';
import { CommentsService } from '../../services/commentsService';

@Component({
  selector: 'app-articlecard',
  templateUrl: './articlecard.component.html',
  styleUrl: './articlecard.component.css',
  standalone: true,
  imports: [
    MatChip,
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule
  ],
  changeDetection: ChangeDetectionStrategy.Default
})

export class ArticlecardComponent {

  // Takes an article as input and displays it as a card
  likes: number = 0;
  comments: number = 0;
  isLikedByUser: boolean = false;

  @Input() categories: Category[] = [];
  @Input() tags: Tag[] = [];
  @Input() article!: Article;


  @Output() update: EventEmitter<Article> = new EventEmitter<Article>();

  constructor(
    private router: Router,
    private articleService: ArticleService,
    public authenticationService: AuthenticationService,
    private httpService: HttpClient,
    private snackBar: MatSnackBar, // Add MatSnackBar service
    private commentService: CommentsService
  ) {}

  ngOnInit() {

    // Get the number of likes of this article
    if (this.article.computedHashId) {
      this.articleService.getNumberOfLikesOfArticle(this.article.computedHashId).subscribe((likes: any) => {
        this.likes = likes;
      });
      this.commentService.getCommentsForArticle(this.article.computedHashId).subscribe(comments => {
        this.comments = comments.length;
      });
    } else {
      console.error('computedHashId is undefined');
    }

    // Determine if the user has liked this article
    const computedHashId = this.article.computedHashId;
    const username = this.authenticationService.getUsername();
    const email = this.authenticationService.getEmail();

    if (computedHashId && username && email) {
      this.articleService.isLikedByUser(computedHashId, username, email).subscribe((isLiked: any) => {
        if (isLiked) {
          this.isLikedByUser = true;
        }
      });

      // Get the tags of the article
      if (this.tags.length === 0) {
        this.articleService.getTagsOfArticle(computedHashId).subscribe((tags: Tag[]) => {
          this.tags = tags;
        });
      }

      // Get the categories of the article
      if (!this.categories || this.categories.length === 0) {
        this.articleService.getCategoriesOfArticle(computedHashId).subscribe((categories: Category[]) => {
          this.categories = categories;
        });
      }

    } else {
      console.error('Missing required information to check like status');
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['article']) {
      // Handle changes if needed
    }
  }

  ngOnDestroy() {}

  viewArticle(article: Article) {
    this.articleService.incrementArticleViews(article.computedHashId).subscribe();
    this.router.navigate(['/article', article.computedHashId]);
  }

  toggleLike() {
    const computedHashId = this.article.computedHashId;
    const username = this.authenticationService.getUsername();
    const email = this.authenticationService.getEmail();

    if (computedHashId && username && email) {
      if (this.isLikedByUser) {
        this.articleService.unlikeArticle(computedHashId, username, email).subscribe(() => {
          this.isLikedByUser = false;
          this.likes--;
        });
      } else {
        this.articleService.likeArticle(computedHashId, username, email).subscribe(() => {
          this.isLikedByUser = true;
          this.likes++;
        });
      }
    } else {
      console.error('Required fields are missing');
    }
  }

  getTagColor(tag: Tag): string {
    // Generate a color based on the tag string
    let hash = 0;
    for (let i = 0; i < tag.tagName!.length; i++) {
      hash = tag.tagName!.charCodeAt(i) + ((hash << 5) - hash);
    }
    const color = `hsl(${hash % 360}, 70%, 70%)`;
    return color;
  }

  notifyUpdate() {
    this.update.emit(this.article);
  }

  redirectToCategory(_t17: Category) {
    this.router.navigate(
      ['/category', _t17.name]
    );
  }

  copyArticleLink() {
    const articleLink = `${window.location.origin}/article/${this.article.computedHashId}`;
    navigator.clipboard.writeText(articleLink).then(() => {
      this.snackBar.open('Link copied to clipboard!', 'Close', {
        duration: 3000, // Duration the snackbar is displayed
      });
    }).catch(err => {
      console.error('Failed to copy the text to clipboard', err);
    });
  }
}

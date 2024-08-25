import { Component, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { ArticleService } from '../../services/articleService';
import { AuthenticationService } from '../../services/authenticationService';
import Quill from 'quill';

@Component({
  selector: 'app-createarticlecomponent',
  templateUrl: './createarticlecomponent.component.html',
  styleUrls: ['./createarticlecomponent.component.css']
})
export class CreatearticlecomponentComponent implements AfterViewInit {
  @ViewChild('quillEditor') quillEditor!: ElementRef;
  localArticle : { title: string; description: string; shortDescription: string; featuredImageURL: string } = {
    title: '',
    description: '',
    shortDescription: '',
    featuredImageURL: ''
  };

  constructor(
    private articleService: ArticleService,
    private router: Router,
    private authenticationService: AuthenticationService,
    private snackBar: MatSnackBar,
    private httpService: HttpClient
  ) {}

  ngAfterViewInit() {
    const quill = new Quill(this.quillEditor.nativeElement, {
      theme: 'bubble',
      modules: {
        toolbar: [
          ['bold', 'italic', 'underline', 'strike'], // toggled buttons
          ['blockquote', 'code-block'],
          [{ header: 1 }, { header: 2 }], // custom button values
          [{ list: 'ordered' }, { list: 'bullet' }],
          [{ script: 'sub' }, { script: 'super' }], // superscript/subscript
          [{ indent: '-1' }, { indent: '+1' }], // outdent/indent
          [{ direction: 'rtl' }], // text direction
          [{ size: ['small', false, 'large', 'huge'] }], // custom dropdown
          [{ header: [1, 2, 3, 4, 5, 6, false] }],
          [{ color: [] }, { background: [] }], // dropdown with defaults from theme
          [{ font: [] }],
          [{ align: [] }],
          ['clean'], // remove formatting button
          ['link', 'image', 'video'] // link and image, video
        ]
      }
    });

    quill.on('text-change', () => {
      this.localArticle.description = quill.root.innerHTML;
    });

    this.applyDarkTheme(quill);
  }

  applyDarkTheme(quill: Quill) {
    const editor = quill.root;
    editor.style.backgroundColor = '#333';
    editor.style.color = '#fff';
    editor.style.minHeight = '200px';
  }

  submitArticle() {
    if (!this.localArticle.title || !this.localArticle.description || !this.localArticle.shortDescription || !this.localArticle.featuredImageURL) {
      this.snackBar.open('Please fill in all fields', 'Close', { duration: 5000 });
      return;
    }

    this.articleService.addArticle(this.localArticle).subscribe(() => {
      this.snackBar.open('Article created', 'Close', { duration: 5000 });
      this.router.navigate(['/']);
    });
  }

  ngOnInit() {}
  ngOnDestroy() {}
}

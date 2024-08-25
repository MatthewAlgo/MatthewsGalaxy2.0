import { AfterViewInit, Component, ElementRef, ViewChild, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Project } from '../../models/project';
import { ProjectService } from '../../services/projectService';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import Quill from 'quill';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-createprojectcomponent',
  templateUrl: './createprojectcomponent.component.html',
  styleUrls: ['./createprojectcomponent.component.css']
})
export class CreateprojectcomponentComponent implements OnInit, AfterViewInit {
  @ViewChild('quillEditor') quillEditor!: ElementRef;
  localProject: Project = new Project();
  file: File | null = null;

  constructor(
    private projectService: ProjectService,
    private router: Router,
    private snackBar: MatSnackBar,
    private formBuilder: FormBuilder,
    private _sanitizer: DomSanitizer
  ) {}

  ngOnInit() {}
  ngAfterViewInit(): void {
    const quill = new Quill(this.quillEditor.nativeElement, {
      theme: 'bubble',
      modules: {
        toolbar: [
          ['bold', 'italic', 'underline', 'strike'],
          ['blockquote', 'code-block'],
          [{ header: 1 }, { header: 2 }],
          [{ list: 'ordered' }, { list: 'bullet' }],
          [{ script: 'sub' }, { script: 'super' }],
          [{ indent: '-1' }, { indent: '+1' }],
          [{ direction: 'rtl' }],
          [{ size: ['small', false, 'large', 'huge'] }],
          [{ header: [1, 2, 3, 4, 5, 6, false] }],
          [{ color: [] }, { background: [] }],
          [{ font: [] }],
          [{ align: [] }],
          ['clean'],
          ['link', 'image', 'video']
        ]
      }
    });

    quill.on('text-change', () => {
      this.localProject.description = quill.root.innerHTML;
    });

    this.applyDarkTheme(quill);
  }

  applyDarkTheme(quill: Quill): void {
    const editor = quill.root;
    editor.style.backgroundColor = '#333';
    editor.style.color = '#fff';
    editor.style.minHeight = '200px';
  }

  uploadImage(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.localProject.localImageLocation = e.target.result; // base64 encoded string
      };
      reader.readAsDataURL(file);
    }
  }

  submitProject(): void {
    // If the project is not valid, return
    if (!this.localProject.isValid()) {
      this.snackBar.open('Please fill out all required fields', 'Close', { duration: 3000 });
      return;
    }

    this.projectService.createProject(this.localProject).subscribe(
      () => {
        this.snackBar.open('Project created successfully!', 'Close', { duration: 3000 });
        this.router.navigate(['/projects']);
      },
      error => {
        this.snackBar.open('Failed to create project', 'Close', { duration: 3000 });
      }
    );
  }
}

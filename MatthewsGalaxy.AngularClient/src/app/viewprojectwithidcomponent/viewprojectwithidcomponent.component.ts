import { Component, Input } from '@angular/core';
import { Project } from '../../models/project';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleService } from '../../services/articleService';
import { CommentsService } from '../../services/commentsService';
import { AuthenticationService } from '../../services/authenticationService';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ProjectService } from '../../services/projectService';

@Component({
  selector: 'app-viewprojectwithidcomponent',
  templateUrl: './viewprojectwithidcomponent.component.html',
  styleUrl: './viewprojectwithidcomponent.component.css'
})
export class ViewprojectwithidcomponentComponent {
  @Input() project : Project | undefined;
  projectId: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private projectService: ProjectService,
    private commentService: CommentsService,
    private authenticationService: AuthenticationService,
    private snackBar: MatSnackBar,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.projectId = params['id'];
      if (!this.project) {
        this.projectService.getProjectById(this.projectId!).subscribe(project => {
          this.project = project;
        });
      }
    });
  }

  ngOnDestroy() {}
}

import { Component, HostListener, OnInit } from '@angular/core';
import { Project } from '../../models/project';
import { ProjectcardcomponentComponent } from '../projectcardcomponent/projectcardcomponent.component';
import { NgxMasonryAnimations } from 'ngx-masonry';
import { NgxMasonryComponent } from 'ngx-masonry';
import { ProjectService } from '../../services/projectService';

@Component({
  selector: 'app-projectspagecomponent',
  templateUrl: './projectspagecomponent.component.html',
  styleUrl: './projectspagecomponent.component.css'
})
export class ProjectspagecomponentComponent implements OnInit {
  projects: Project[] = [];

  constructor(private projectsService: ProjectService) {}

  ngOnInit() {
    this.projectsService.getProjects().subscribe((data: Project[]) => {
      this.projects = data;
    });
  }

  ngOnDestroy() {}

}

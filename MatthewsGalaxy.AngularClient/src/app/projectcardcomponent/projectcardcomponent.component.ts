import { Component, HostListener, Input } from '@angular/core';
import { Project } from '../../models/project';
import { SlicePipe } from '@angular/common';
import { NgxMasonryAnimations } from 'ngx-masonry';
import  { NgxMasonryComponent } from 'ngx-masonry';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-projectcardcomponent',
  templateUrl: './projectcardcomponent.component.html',
  styleUrl: './projectcardcomponent.component.css'
})
export class ProjectcardcomponentComponent {

  @Input() project : Project | undefined;
  imageData: SafeResourceUrl | undefined;

  constructor(private _sanitizer: DomSanitizer, private router: Router) {}

  ngOnInit() {
    this.imageData = this._sanitizer.bypassSecurityTrustResourceUrl(this.project?.localImageLocation!);
  }

  ngOnDestroy() {}

  viewProject() {
    this.router.navigate(['/project', this.project?.id]);
  }


}


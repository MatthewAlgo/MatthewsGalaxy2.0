import { Component } from '@angular/core';
import { ApplicationblogintroComponent } from './applicationblogintro/applicationblogintro.component';
import { ArticleviewcomponentComponent } from '../articleviewcomponent/articleviewcomponent.component';

@Component({
  selector: 'app-mainapplicationbody',
  templateUrl: './mainapplicationbody.component.html',
  styleUrl: './mainapplicationbody.component.css',
  standalone: true,
  imports: [
    ApplicationblogintroComponent,
    ArticleviewcomponentComponent
  ]
})
export class MainapplicationbodyComponent {

}

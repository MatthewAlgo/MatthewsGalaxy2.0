import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainapplicationbodyComponent } from './mainapplicationbody/mainapplicationbody.component';
import { LoginviewcomponentComponent } from './loginviewcomponent/loginviewcomponent.component';
import { RegisterviewcomponentComponent } from './registerviewcomponent/registerviewcomponent.component';
import { ArticleviewcomponentComponent } from './articleviewcomponent/articleviewcomponent.component';
import { ViewarticlewithidcomponentComponent } from './viewarticlewithidcomponent/viewarticlewithidcomponent.component';
import { AdminpanelcomponentComponent } from './adminpanelcomponent/adminpanelcomponent.component';
import { CreatearticlecomponentComponent } from './createarticlecomponent/createarticlecomponent.component';
import { AboutmecomponentComponent } from './aboutmecomponent/aboutmecomponent.component';
import { ProjectspagecomponentComponent } from './projectspagecomponent/projectspagecomponent.component';
import { CreateprojectcomponentComponent } from './createprojectcomponent/createprojectcomponent.component';
import { CategoriesviewcomponentComponent } from './categoriesviewcomponent/categoriesviewcomponent.component';
import { CategoriesforarticlescomponentComponent } from './categoriesforarticlescomponent/categoriesforarticlescomponent.component';
import { ViewprojectwithidcomponentComponent } from './viewprojectwithidcomponent/viewprojectwithidcomponent.component';

const routes: Routes = [
  { path: '', component: MainapplicationbodyComponent, title: 'Matthew\'s Galaxy' },
  { path: 'login', component: LoginviewcomponentComponent, title: 'Login Page'},
  { path: 'register', component: RegisterviewcomponentComponent, title: 'Login / Register'},
  { path: 'article/:id', component: ViewarticlewithidcomponentComponent, title: 'Article Page'},
  { path: 'adminpanel' , component: AdminpanelcomponentComponent, title: 'Admin Panel'},
  { path: 'create-article', component: CreatearticlecomponentComponent, title: 'Create Article'},
  { path: 'about-me', component: AboutmecomponentComponent, title: 'About Me'},
  { path: 'my-projects', component: ProjectspagecomponentComponent, title: 'My Projects'},
  { path: 'create-project', component: CreateprojectcomponentComponent, title: 'Create Project'},
  { path: 'categories', component: CategoriesviewcomponentComponent, title: 'Categories'},
  { path: 'category/:categoryName', component: CategoriesforarticlescomponentComponent, title: 'Category Details'},
  { path: 'project/:id', component: ViewprojectwithidcomponentComponent, title: 'Project Details'},
  { path: '**', redirectTo: '' }
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}

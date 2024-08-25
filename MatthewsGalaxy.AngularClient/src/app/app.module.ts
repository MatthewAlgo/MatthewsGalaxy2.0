import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule, DatePipe, NgIf, SlicePipe } from '@angular/common';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ApplicationheaderComponent } from './applicationheader/applicationheader.component';
import { MainapplicationbodyComponent } from './mainapplicationbody/mainapplicationbody.component';
import { LoginviewcomponentComponent } from './loginviewcomponent/loginviewcomponent.component';
import { RegisterviewcomponentComponent } from './registerviewcomponent/registerviewcomponent.component';
import { ApplicationfooterComponent } from './applicationfooter/applicationfooter.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { ArticlecardComponent } from './articlecard/articlecard.component';
import { ArticleviewcomponentComponent } from './articleviewcomponent/articleviewcomponent.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

// Ignite UI for Angular Components
import { IgxNavigationDrawerModule, IgxToggleModule, IgxButtonModule, IgxRippleModule, IgxAvatarModule, IgxInputGroupModule, IgxIconModule } from 'igniteui-angular';
import { ViewarticlewithidcomponentComponent } from './viewarticlewithidcomponent/viewarticlewithidcomponent.component';
import { MarkdownModule } from 'ngx-markdown';
import { AuthenticationService } from '../services/authenticationService';
import { AuthInterceptor } from '../services/authInterceptorService';
import { DialogwithinputscomponentComponent } from './dialogwithinputscomponent/dialogwithinputscomponent.component';
import { QuillConfigModule, QuillModule } from 'ngx-quill';

// PrimeNG Components
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MatChipsModule } from '@angular/material/chips';
import { AdminpanelcomponentComponent } from './adminpanelcomponent/adminpanelcomponent.component';
import { NgxMasonryComponent, NgxMasonryModule } from 'ngx-masonry';
import { UsercardComponent } from './usercard/usercard.component';
import { CreatearticlecomponentComponent } from './createarticlecomponent/createarticlecomponent.component';
import { RECAPTCHA_SETTINGS, RECAPTCHA_V3_SITE_KEY, RecaptchaFormsModule, RecaptchaModule, RecaptchaSettings } from 'ng-recaptcha';
import { environment } from '../env/environment';
import { AboutmecomponentComponent } from './aboutmecomponent/aboutmecomponent.component';
import { ProjectcardcomponentComponent } from './projectcardcomponent/projectcardcomponent.component';
import { ProjectspagecomponentComponent } from './projectspagecomponent/projectspagecomponent.component';
import { CreateprojectcomponentComponent } from './createprojectcomponent/createprojectcomponent.component';
import { ViewprojectwithidcomponentComponent } from './viewprojectwithidcomponent/viewprojectwithidcomponent.component';
import { CategoriesviewcomponentComponent } from './categoriesviewcomponent/categoriesviewcomponent.component';
import { CategoriesforarticlescomponentComponent } from './categoriesforarticlescomponent/categoriesforarticlescomponent.component';

@NgModule({
  declarations: [
    AppComponent,
    ViewarticlewithidcomponentComponent,
    LoginviewcomponentComponent,
    RegisterviewcomponentComponent,
    DialogwithinputscomponentComponent,
    AdminpanelcomponentComponent,
    CreatearticlecomponentComponent,
    AboutmecomponentComponent,
    ProjectcardcomponentComponent,
    ProjectspagecomponentComponent,
    CreateprojectcomponentComponent,
    ViewprojectwithidcomponentComponent,
    CategoriesviewcomponentComponent,
    CategoriesforarticlescomponentComponent,
    ApplicationfooterComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    MatToolbarModule,
    MatIconModule,
    BrowserAnimationsModule,
    IgxNavigationDrawerModule,
    IgxButtonModule,
    IgxRippleModule,
    IgxAvatarModule,
    IgxInputGroupModule,
    IgxIconModule,
    IgxToggleModule,
    MarkdownModule.forRoot(),
    MatDialogModule,
    MatFormFieldModule,
    NgxMasonryModule, // Add NgxMasonryModule to imports
    MatChipsModule,
    MatInputModule,
    MatButtonModule,
    ReactiveFormsModule,
    DialogModule,
    ButtonModule,
    InputTextModule,
    CommonModule,
    RecaptchaModule,
    SlicePipe,
    QuillModule.forRoot(),
    QuillConfigModule.forRoot({
      modules: {
        syntax: true,
        toolbar: [
          ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
          ['blockquote', 'code-block'],

          [{ 'header': 1 }, { 'header': 2 }],               // custom button values
          [{ 'list': 'ordered'}, { 'list': 'bullet' }],
          [{ 'script': 'sub'}, { 'script': 'super' }],      // superscript/subscript
          [{ 'indent': '-1'}, { 'indent': '+1' }],          // outdent/indent
          [{ 'direction': 'rtl' }],                         // text direction

          [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
          [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

          [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
          [{ 'font': [] }],
          [{ 'align': [] }],

          ['clean'],                                         // remove formatting button

          ['link', 'image', 'video']                         // link and image, video
        ]
      }
    }),
    ApplicationheaderComponent,
    ArticlecardComponent,
    UsercardComponent,
    RecaptchaModule,
    RecaptchaFormsModule,
    NgIf,
],
  providers: [
    provideAnimationsAsync(),
    AuthenticationService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    DatePipe,
    {
      provide: RECAPTCHA_SETTINGS,
      useValue: {
        siteKey: environment.recaptcha.siteKey,
      } as RecaptchaSettings,
    },
    NgxMasonryComponent,
    NgxMasonryModule,
  ],
  bootstrap: [
    AppComponent
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ]
})
export class AppModule { }

import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card'; // Add this import statement
import { MatChip } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { IgxAvatarModule } from 'igniteui-angular'; // Add this import statement
import { IgxIconModule } from 'igniteui-angular'; // Add this import statement
import { IgxButtonModule } from 'igniteui-angular'; // Add this import statement
import { IgxRippleModule } from 'igniteui-angular'; // Add this import statement
import { IgxToggleModule } from 'igniteui-angular'; // Add this import statement
import { ArticleService } from '../../services/articleService';
import { AuthenticationService } from '../../services/authenticationService';
import { UserService } from '../../services/userService';
import { IdentityUser } from '../../models/user';

@Component({
  selector: 'app-usercard',
  templateUrl: './usercard.component.html',
  styleUrl: './usercard.component.css',
  standalone: true,
  imports: [
    MatChip,
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule
  ],

})
export class UsercardComponent {

  @Input() identityUser: IdentityUser | undefined;

  constructor(private router: Router, private articleService: ArticleService, public authenticationService: AuthenticationService,
    private userService : UserService
  ) {}

  ngOnInit() {}

  ngOnDestroy() {}
}

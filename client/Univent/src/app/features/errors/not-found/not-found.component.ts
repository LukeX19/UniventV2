import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { Router, RouterModule } from '@angular/router';
import { TokenService } from '../../../core/services/token.service';

@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    RouterModule
  ],
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.scss'
})
export class NotFoundComponent {
  private router = inject(Router);
  private tokenService = inject(TokenService);

  handleRedirect(): void {
    const token = localStorage.getItem('uniapi-token');

    if (!token) {
      this.router.navigateByUrl('/');
      return;
    }

    const role = this.tokenService.getUserRole(token);

    switch (role) {
      // Admin
      case 0:
        this.router.navigateByUrl('/admin/dashboard');
        break;
      // Student
      case 1:
        this.router.navigateByUrl('/home');
        break;
      default:
        this.router.navigateByUrl('/');
    }
  }
}

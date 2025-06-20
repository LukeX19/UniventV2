import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { Router, RouterModule } from '@angular/router';
import { TokenService } from '../../../core/services/token.service';
import { CustomButtonComponent } from '../../../shared/components/custom-button/custom-button.component';

@Component({
  selector: 'app-forbidden',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    RouterModule,
    CustomButtonComponent
  ],
  templateUrl: './forbidden.component.html',
  styleUrl: './forbidden.component.scss'
})
export class ForbiddenComponent {
  private router = inject(Router);
    private tokenService = inject(TokenService);
  
    handleRedirect(): void {
      const token = localStorage.getItem('uniapi-token');
  
      if (!token) {
        this.router.navigate(['/']);
        return;
      }
  
      const role = this.tokenService.getUserRole(token);
  
      switch (role) {
        // Admin
        case 0:
          this.router.navigate(['/admin/dashboard']);
          break;
        // Student
        case 1:
          this.router.navigate(['/browse']);
          break;
        default:
          this.router.navigate(['/']);
      }
    }
}

import { Component, ElementRef, HostListener, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { AuthenticationService } from '../../../core/services/authentication.service';
import { Observable } from 'rxjs';
import { UserResponse } from '../../models/userModel';
import { Router } from '@angular/router';
import { TokenService } from '../../../core/services/token.service';
import { CustomButtonComponent } from '../custom-button/custom-button.component';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    CommonModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    CustomButtonComponent
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  private router = inject(Router);
  private authService = inject(AuthenticationService);
  private tokenService = inject(TokenService);

  user$: Observable<UserResponse | null> = this.authService.user$;
  
  isProfileMenuOpen = false;
  isBurgerMenuOpen = false;

  constructor(private eRef: ElementRef) {}

  toggleProfileMenu() {
    this.isProfileMenuOpen = !this.isProfileMenuOpen;
  }

  toggleBurgerMenu() {
    this.isBurgerMenuOpen = !this.isBurgerMenuOpen;
  }

  @HostListener('document:click', ['$event'])
  handleClickOutside(event: Event) {
    if (!this.eRef.nativeElement.contains(event.target)) {
      this.isProfileMenuOpen = false;
      this.isBurgerMenuOpen = false;
    }
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['']);
  }

  getUserInitials(user: UserResponse | null): string {
    if (!user) return 'U';
    const firstNameInitial = user.firstName ? user.firstName.charAt(0).toUpperCase() : '';
    const lastNameInitial = user.lastName ? user.lastName.charAt(0).toUpperCase() : '';
    return `${firstNameInitial}${lastNameInitial}`;
  }

  goToEventCreate() {
    this.router.navigate(['/host']);
  }

  goToEventsBrowse() {
    this.router.navigate(['/browse']);
  }

  goToMyProfile(id: string) {
    this.router.navigate([`/profile/${id}`]);
  }

  get isAdmin(): boolean {
    const token = localStorage.getItem('uniapi-token');
    const role = this.tokenService.getUserRole(token ?? '');
    return role === 0;
  }
}

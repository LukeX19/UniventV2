import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { UserService } from '../../core/services/user.service';
import { UserProfileResponse } from '../../shared/models/userModel';
import { ActivatedRoute } from '@angular/router';
import { NavbarComponent } from '../../shared/components/navbar/navbar.component';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    NavbarComponent,
    MatIconModule
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
  private userService = inject(UserService);
  private route = inject(ActivatedRoute);

  user: UserProfileResponse | null = null;
  isLoading = true;

  ngOnInit() {
    this.fetchUser();
  }

  fetchUser() {
    const userId = this.route.snapshot.paramMap.get('id');
    if (!userId) return;

    this.isLoading = true;
    this.userService.fetchUserProfileById(userId).subscribe({
      next: (data) => {
        this.user = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error("Error fetching user:", error);
        this.isLoading = false;
      }
    });
  }

  getUserInitials(user: UserProfileResponse | null): string {
      if (!user) return 'U';
      const firstNameInitial = user.firstName ? user.firstName.charAt(0).toUpperCase() : '';
      const lastNameInitial = user.lastName ? user.lastName.charAt(0).toUpperCase() : '';
      return `${firstNameInitial}${lastNameInitial}`;
    }

  get formattedDate(): string {
    if (!this.user) return "";
    
    const createdAt = new Date(this.user.createdAt).toLocaleString("ro-RO", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric"
    });

    return `${createdAt}`;
  }

  getAccountAge(createdAt?: string): string {
    if (!createdAt) return '';
    const createdDate = new Date(createdAt);
    const now = new Date();
    const diffInMs = now.getTime() - createdDate.getTime();
    const diffInYears = Math.floor(diffInMs / (1000 * 60 * 60 * 24 * 365));
    if (diffInYears > 0) return `${diffInYears} year${diffInYears > 1 ? 's' : ''}`;
    const diffInMonths = Math.floor(diffInMs / (1000 * 60 * 60 * 24 * 30));
    return `${diffInMonths} month${diffInMonths !== 1 ? 's' : ''}`;
  }

  yearMapper(year?: number): string {
    if (!year) return '';
    const map: { [key: number]: string } = { 1: 'I', 2: 'II', 3: 'III', 4: 'IV', 5: 'V', 6: 'VI', 7: 'I Master', 8: 'II Master' };
    return map[year] || `${year}`;
  }
}

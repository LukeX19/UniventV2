import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { UserProfileResponse, UserResponse } from '../../shared/models/userModel';
import { ActivatedRoute, Router } from '@angular/router';
import { NavbarComponent } from '../../shared/components/navbar/navbar.component';
import { MatIconModule } from '@angular/material/icon';
import { EventSummaryResponse, EventSummaryWithFeedbackStatusResponse } from '../../shared/models/eventModel';
import { EventService } from '../../core/services/event.service';
import { PaginationRequest } from '../../shared/models/paginationModel';
import { EventCardComponent } from "../../shared/components/event-card/event-card.component";
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { AuthenticationService } from '../../core/services/authentication.service';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { SnackbarService } from '../../core/services/snackbar.service';
import { UserService } from '../../core/services/user.service';
import { GenericDialogComponent } from '../../shared/components/generic-dialog/generic-dialog.component';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    NavbarComponent,
    MatIconModule,
    MatButtonModule,
    EventCardComponent,
    MatPaginatorModule
],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent {
  private userService = inject(UserService);
  private authService = inject(AuthenticationService);
  private eventService = inject(EventService);
  private dialog = inject(MatDialog);
  private snackbar = inject(SnackbarService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  user: UserProfileResponse | null = null;
  isUserLoading = true;

  currentUser: UserResponse | null = null;

  showCreatedSection = true;

  createdEvents: EventSummaryResponse[] = [];
  areCreatedEventsLoading = true;

  participatedEvents: EventSummaryWithFeedbackStatusResponse[] = [];
  areParticipatedEventsLoading = true;

  createdPagination: PaginationRequest = { pageIndex: 1, pageSize: 10 };
  createdTotalEvents = 0;
  createdTotalPages = 1;

  participatedPagination: PaginationRequest = { pageIndex: 1, pageSize: 10 };
  participatedTotalEvents = 0;
  participatedTotalPages = 1;

  ngOnInit() {
    this.authService.user$.subscribe((currentUser) => {
      this.currentUser = currentUser;
    });

    this.route.paramMap.subscribe(() => {
      const resolvedUser = this.route.snapshot.data['user'] as UserProfileResponse | null;

      if (!resolvedUser) {
        this.router.navigate(['/not-found']);
        return;
      }

      this.user = resolvedUser;
      this.isUserLoading = false;

      this.createdPagination.pageIndex = 1;
      this.participatedPagination.pageIndex = 1;

      this.fetchCreatedEvents(resolvedUser.id);
      if (!this.showCreatedSection) {
        this.fetchParticipatedEvents(resolvedUser.id);
      }

      this.showCreatedSection = true;
      this.fetchCreatedEvents(resolvedUser.id);
    });
  }

  fetchCreatedEvents(userId: string) {
    this.areCreatedEventsLoading = true;
  
    this.eventService.fetchCreatedEventsSummariesByUserId(userId, this.createdPagination).subscribe({
      next: (data) => {
        this.createdEvents = data.elements;
        this.createdTotalEvents = data.resultsCount;
        this.createdTotalPages = data.totalPages;
        this.createdPagination.pageIndex = data.pageIndex;
        this.areCreatedEventsLoading = false;
      },
      error: (error) => {
        console.error("Error fetching created events:", error);
        this.areCreatedEventsLoading = false;
      }
    });
  }
  
  fetchParticipatedEvents(userId: string) {
    this.areParticipatedEventsLoading = true;
  
    this.eventService.fetchParticipatedEventsSummariesByUserId(userId, this.participatedPagination).subscribe({
      next: (data) => {
        this.participatedEvents = data.elements;
        this.participatedTotalEvents = data.resultsCount;
        this.participatedTotalPages = data.totalPages;
        this.participatedPagination.pageIndex = data.pageIndex;
        this.areParticipatedEventsLoading = false;
      },
      error: (error) => {
        console.error("Error fetching participated events:", error);
        this.areParticipatedEventsLoading = false;
      }
    });
  }

  onCreatedPageChange(event: PageEvent) {
    if (!this.user) return;
  
    this.createdPagination.pageIndex = event.pageIndex + 1;
    this.createdPagination.pageSize = event.pageSize;
    this.fetchCreatedEvents(this.user.id);
  }
  
  onParticipatedPageChange(event: PageEvent) {
    if (!this.user) return;
  
    this.participatedPagination.pageIndex = event.pageIndex + 1;
    this.participatedPagination.pageSize = event.pageSize;
    this.fetchParticipatedEvents(this.user.id);
  }
  

  getUserInitials(user: UserProfileResponse | null): string {
      if (!user) return 'U';
      const firstNameInitial = user.firstName ? user.firstName.charAt(0).toUpperCase() : '';
      const lastNameInitial = user.lastName ? user.lastName.charAt(0).toUpperCase() : '';
      return `${firstNameInitial}${lastNameInitial}`;
    }

  get formattedDate(): string {
    if (!this.user) return "";
    
    const birthday = new Date(this.user.birthday).toLocaleString("ro-RO", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric"
    });

    return `${birthday}`;
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
    const map: { [key: number]: string } = { 1: 'Year I', 2: 'Year II', 3: 'Year III', 4: 'Year IV', 5: 'Year V', 6: 'Year VI', 7: 'Master Year I', 8: 'Master Year II' };
    return map[year] || `${year}`;
  }

  switchSection(showCreated: boolean) {
    this.showCreatedSection = showCreated;
  
    if (!showCreated && this.participatedEvents.length === 0 && this.user) {
      this.fetchParticipatedEvents(this.user.id);
    }
  }

  get isOwnProfile(): boolean {
    return this.currentUser?.id === this.user?.id;
  }

  onEditProfile() {
    this.router.navigate([`/profile/${this.currentUser!.id}/edit`]);
  }

  onDeleteAccount() {
    const dialogRef = this.dialog.open(GenericDialogComponent, {
    data: {
      title: 'Delete Account',
      message: 'Are you sure you want to delete your account? This action is permanent and cannot be undone.',
      cancelText: 'Cancel',
      confirmText: 'Delete'
    }
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      this.userService.deleteUser().subscribe({
        next: () => {
          this.snackbar.success('Your account has been deleted.');
          this.authService.logout();
          this.router.navigate(['/login']);
        },
        error: (error) => {
          this.snackbar.error('Something went wrong. Please try again.');
          console.error("Failed to delete account:", error);
        }
      });
    }
  });
  }
}

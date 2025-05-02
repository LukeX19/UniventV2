import { Component, inject } from '@angular/core';
import { NavbarComponent } from '../../shared/components/navbar/navbar.component';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { UserService } from '../../core/services/user.service';
import { UserManagementResponse } from '../../shared/models/userModel';
import { PaginationRequest } from '../../shared/models/paginationModel';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { SnackbarService } from '../../core/services/snackbar.service';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatPaginatorModule,
    NavbarComponent
  ],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.scss'
})
export class AdminDashboardComponent {
  private userService = inject(UserService);
  private snackbarService = inject(SnackbarService);

  users: UserManagementResponse[] = [];
  pagination: PaginationRequest = { pageIndex: 1, pageSize: 10 };
  totalUsers = 0;
  
  selectedTab: 'users' | 'eventTypes' | 'universities' = 'users';

  ngOnInit() {
    this.fetchUsers();
  }

  selectTab(tab: 'users' | 'eventTypes' | 'universities') {
    this.selectedTab = tab;
  }

  fetchUsers(): void {
    this.userService.fetchAllUsers(this.pagination).subscribe({
      next: (data) => {
        this.users = data.elements;
        this.totalUsers = data.resultsCount;
        this.pagination.pageIndex = data.pageIndex;
      }
    });
  }

  onPageChange(event: PageEvent) {
    this.pagination.pageIndex = event.pageIndex + 1;
    this.pagination.pageSize = event.pageSize;
    this.fetchUsers();
  }

  getStatusInfo(user: UserManagementResponse): { label: string, color: string, icon: string } {
    if (user.isAccountBanned) return { label: 'Banned', color: 'text-red-600', icon: 'block' };
    if (!user.isAccountConfirmed) return { label: 'Pending', color: 'text-yellow-600', icon: 'schedule' };
    return { label: 'Approved', color: 'text-green-600', icon: 'check_circle' };
  }

  approveUser(id: string) {
    this.userService.approveUser(id).subscribe({
      next: () => {
        this.snackbarService.success("User approved successfully.");
        this.fetchUsers();
      },
      error: (error) => {
        console.error("Failed to approve user:", error);
        this.snackbarService.error("Something went wrong. Please try again later.");
      }
    });
  }
  
  banUser(id: string) {
    this.userService.banUser(id).subscribe({
      next: () => {
        this.snackbarService.success("User banned successfully.");
        this.fetchUsers();
      },
      error: (error) => {
        console.error("Failed to ban user:", error);
        this.snackbarService.error("Something went wrong. Please try again later.");
      }
    });
  }
}

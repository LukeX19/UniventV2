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
import { UniversityService } from '../../core/services/university.service';
import { UniversityResponse, UniversityRequest } from '../../shared/models/universityModel';
import { MatDialog } from '@angular/material/dialog';
import { EditTextDialogComponent } from '../../shared/components/edit-text-dialog/edit-text-dialog.component';
import { GenericDialogComponent } from '../../shared/components/generic-dialog/generic-dialog.component';
import { EventTypeService } from '../../core/services/event-type.service';
import { EventTypeRequest, EventTypeResponse } from '../../shared/models/eventTypeModel';

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
  private universityService = inject(UniversityService);
  private eventTypeService = inject(EventTypeService);
  private snackbarService = inject(SnackbarService);
  private dialog = inject(MatDialog);

  users: UserManagementResponse[] = [];
  usersPagination: PaginationRequest = { pageIndex: 1, pageSize: 10 };
  totalUsers = 0;

  universities: UniversityResponse[] = [];
  universityPagination: PaginationRequest = { pageIndex: 1, pageSize: 10 };
  totalUniversities = 0;

  eventTypes: EventTypeResponse[] = [];
  eventTypePagination: PaginationRequest = { pageIndex: 1, pageSize: 10 };
  totalEventTypes = 0;
  
  selectedTab: 'users' | 'eventTypes' | 'universities' = 'users';

  ngOnInit() {
    this.fetchUsers();
    this.fetchUniversities();
    this.fetchEventTypes();
  }

  selectTab(tab: 'users' | 'eventTypes' | 'universities') {
    this.selectedTab = tab;
  }

  fetchUsers(): void {
    this.userService.fetchAllUsers(this.usersPagination).subscribe({
      next: (data) => {
        this.users = data.elements;
        this.totalUsers = data.resultsCount;
        this.usersPagination.pageIndex = data.pageIndex;
      },
      error: (error) => {
        console.error("Failed to load users:", error);
      }
    });
  }

  onPageChange(event: PageEvent) {
    this.usersPagination.pageIndex = event.pageIndex + 1;
    this.usersPagination.pageSize = event.pageSize;
    this.fetchUsers();
  }

  getStatusInfo(user: UserManagementResponse): { label: string, color: string, icon: string } {
    if (user.isAccountBanned) return { label: 'Banned', color: 'text-red-600', icon: 'block' };
    if (!user.isAccountConfirmed) return { label: 'Pending', color: 'text-yellow-600', icon: 'schedule' };
    return { label: 'Approved', color: 'text-green-600', icon: 'check_circle' };
  }

  getUniversityYearLabel(year: number): string {
    switch (year) {
      case 1: return 'Year I';
      case 2: return 'Year II';
      case 3: return 'Year III';
      case 4: return 'Year IV';
      case 5: return 'Year V';
      case 6: return 'Year VI';
      case 7: return 'Master Year I';
      case 8: return 'Master Year II';
      default: return 'N/A';
    }
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

  fetchUniversities(): void {
    this.universityService.fetchAllUniversities(this.universityPagination).subscribe({
      next: (data) => {
        this.universities = data.elements;
        this.totalUniversities = data.resultsCount;
        this.universityPagination.pageIndex = data.pageIndex;
      },
      error: (error) => {
        console.error("Failed to load universities:", error);
      }
    });
  }
  
  onUniversityPageChange(event: PageEvent) {
    this.universityPagination.pageIndex = event.pageIndex + 1;
    this.universityPagination.pageSize = event.pageSize;
    this.fetchUniversities();
  }

  editUniversity(id: string): void {
    const university = this.universities.find(u => u.id === id);
    if (!university) return;
  
    const dialogRef = this.dialog.open(EditTextDialogComponent, {
      width: '400px',
      maxWidth: '90vw',
      data: {
        title: 'Edit University',
        initialValue: university.name,
        confirmText: 'Update',
        cancelText: 'Cancel'
      }
    });
  
    dialogRef.afterClosed().subscribe(newName => {
      if (newName && newName !== university.name) {
        const updateRequest : UniversityRequest = { name: newName };
        this.universityService.updateUniversity(id, updateRequest).subscribe({
          next: () => {
            this.snackbarService.success("University updated successfully.");
            this.fetchUniversities();
          },
          error: (error) => {
            const message = error.error?.message;
            
            if (error.status === 409 && message?.includes('already exists')) {
              this.snackbarService.error('A university with that name already exists.');
            } else {
              console.error('Failed to edit university:', error);
              this.snackbarService.error('Something went wrong. Please try again.');
            }
          }
        });
      }
    });
  }

  deleteUniversity(id: string): void {
    const university = this.universities.find(u => u.id === id);
    if (!university) return;
  
    const dialogRef = this.dialog.open(GenericDialogComponent, {
      width: '400px',
      maxWidth: '90vw',
      data: {
        title: 'Confirm Deletion',
        message: `Are you sure you want to permanently delete "${university.name}" from the platform? This action cannot be undone.`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });
  
    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.universityService.deleteUniversity(id).subscribe({
          next: () => {
            this.snackbarService.success('University deleted successfully.');
            this.fetchUniversities();
          },
          error: (error) => {
            console.error('Delete failed:', error);
            this.snackbarService.error('Something went wrong. Please try again.');
          }
        });
      }
    });
  }

  addUniversity(): void {
    const dialogRef = this.dialog.open(EditTextDialogComponent, {
      width: '400px',
      maxWidth: '90vw',
      data: {
        title: 'Add University',
        initialValue: '',
        confirmText: 'Submit',
        cancelText: 'Cancel'
      }
    });
  
    dialogRef.afterClosed().subscribe(newName => {
      if (newName && newName.trim()) {
        const newUniversity: UniversityRequest = { name: newName.trim() };
        this.universityService.createUniversity(newUniversity).subscribe({
          next: () => {
            this.snackbarService.success('University added successfully.');
            this.fetchUniversities();
          },
          error: (error) => {
            const message = error.error?.message;

            if (error.status === 409 && message?.includes('already exists')) {
              this.snackbarService.error('A university with that name already exists.');
            } else {
              console.error('Failed to add university:', error);
              this.snackbarService.error('Something went wrong. Please try again.');
            }
          }
        });
      }
    });
  }

  fetchEventTypes(): void {
    this.eventTypeService.fetchAllEventTypes(this.eventTypePagination).subscribe({
      next: (data) => {
        this.eventTypes = data.elements;
        this.totalEventTypes = data.resultsCount;
        this.eventTypePagination.pageIndex = data.pageIndex;
      },
      error: (error) => {
        console.error("Failed to load event types:", error);
      }
    });
  }

  onEventTypePageChange(event: PageEvent) {
    this.eventTypePagination.pageIndex = event.pageIndex + 1;
    this.eventTypePagination.pageSize = event.pageSize;
    this.fetchEventTypes();
  }

  editEventType(id: string): void {
    const type = this.eventTypes.find(e => e.id === id);
    if (!type) return;
  
    const dialogRef = this.dialog.open(EditTextDialogComponent, {
      width: '400px',
      maxWidth: '90vw',
      data: {
        title: 'Edit Event Type',
        initialValue: type.name,
        confirmText: 'Update',
        cancelText: 'Cancel'
      }
    });
  
    dialogRef.afterClosed().subscribe(newName => {
      if (newName && newName !== type.name) {
        const updateRequest: EventTypeRequest = { name: newName };
        this.eventTypeService.updateEventType(id, updateRequest).subscribe({
          next: () => {
            this.snackbarService.success("Event type updated successfully.");
            this.fetchEventTypes();
          },
          error: (error) => {
            const message = error.error?.message;
            if (error.status === 409 && message?.includes("already exists")) {
              this.snackbarService.error("An event type with that name already exists.");
            } else {
              console.error("Failed to update event type:", error);
              this.snackbarService.error("Something went wrong. Please try again.");
            }
          }
        });
      }
    });
  }

  toggleEventType(type: EventTypeResponse): void {
    const toggleAction = type.isDeleted
      ? this.eventTypeService.enableEventType(type.id)
      : this.eventTypeService.disableEventType(type.id);
  
    toggleAction.subscribe({
      next: () => {
        const message = type.isDeleted
          ? 'Event type re-enabled successfully.'
          : 'Event type disabled successfully.';
        this.snackbarService.success(message);
        this.fetchEventTypes();
      },
      error: (error) => {
        console.error('Failed to toggle event type:', error);
        this.snackbarService.error('Something went wrong. Please try again.');
      }
    });
  }

  addEventType(): void {
    const dialogRef = this.dialog.open(EditTextDialogComponent, {
      width: '400px',
      maxWidth: '90vw',
      data: {
        title: 'Add Event Type',
        initialValue: '',
        confirmText: 'Submit',
        cancelText: 'Cancel'
      }
    });
  
    dialogRef.afterClosed().subscribe(newName => {
      if (newName && newName.trim()) {
        const newType: EventTypeRequest = { name: newName.trim() };
        this.eventTypeService.createEventType(newType).subscribe({
          next: () => {
            this.snackbarService.success("Event type added successfully.");
            this.fetchEventTypes();
          },
          error: (error) => {
            const message = error.error?.message;
            if (error.status === 409 && message?.includes("already exists")) {
              this.snackbarService.error("An event type with that name already exists.");
            } else {
              console.error("Failed to add event type:", error);
              this.snackbarService.error("Something went wrong. Please try again.");
            }
          }
        });
      }
    });
  }
}

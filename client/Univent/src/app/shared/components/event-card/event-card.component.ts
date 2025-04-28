import { Component, inject, Input } from '@angular/core';
import { EventAuthorResponse, EventSummaryResponse } from '../../models/eventModel';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { GenericDialogComponent } from '../generic-dialog/generic-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { EventService } from '../../../core/services/event.service';
import { SnackbarService } from '../../../core/services/snackbar.service';

@Component({
  selector: 'app-event-card',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatMenuModule
  ],
  templateUrl: './event-card.component.html',
  styleUrl: './event-card.component.scss'
})
export class EventCardComponent {
  private router = inject(Router);
  private dialog = inject(MatDialog);
  private eventService = inject(EventService);
  private snackbar = inject(SnackbarService);
  
  @Input() event!: EventSummaryResponse;
  @Input() enableOptions: boolean = false;

  getFormattedStartTime(): string {
    const startTime = new Date(this.event.startTime).toLocaleString("ro-RO", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
      hour: "2-digit",
      minute: "2-digit",
      hour12: false
    });
  
    return `Starts ${startTime}`;
  }

  getFormattedPostedDate(): string {
    const createdAt = new Date(this.event.createdAt).toLocaleString("ro-RO", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
      hour: "2-digit",
      minute: "2-digit",
      hour12: false
    });
  
    const updatedAt = new Date(this.event.updatedAt).toLocaleString("ro-RO", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
      hour: "2-digit",
      minute: "2-digit",
      hour12: false
    });
  
    return updatedAt > createdAt ? `Last updated on ${updatedAt}` : `Posted on ${createdAt}`;
  }

  getAuthorFullName(): string {
    return `${this.event.author.firstName} ${this.event.author.lastName}`;
  }

  getAuthorInitials(user: EventAuthorResponse | null): string {
    if (!user) return 'U';
    const firstNameInitial = user.firstName ? user.firstName.charAt(0).toUpperCase() : '';
    const lastNameInitial = user.lastName ? user.lastName.charAt(0).toUpperCase() : '';
    return `${firstNameInitial}${lastNameInitial}`;
  }

  navigateToEvent() {
    this.router.navigate([`/event/${this.event.id}`]);
  }

  onUpdate() {
    this.router.navigate([`/event/${this.event.id}/update`]);
  }
  
  onCancel() {
    const dialogRef = this.dialog.open(GenericDialogComponent, {
      data: {
        title: 'Cancel Event',
        message: 'Are you sure you want to cancel this event? This action cannot be undone.',
        cancelText: 'No, Keep It',
        confirmText: 'Yes, Cancel'
      }
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.eventService.cancelEvent(this.event.id).subscribe({
          next: () => {
            this.event.isCancelled = true;
            this.snackbar.success("Event successfully cancelled.")
          },
          error: () => this.snackbar.error("Failed to cancel the event.")
        });
      }
    });
  }

  get canEditOrCancel(): boolean {
    if (!this.event) return false;

    const now = new Date();
    const eventStart = new Date(this.event.startTime);

    const diffInMs = eventStart.getTime() - now.getTime();
    const diffInHours = diffInMs / (1000 * 60 * 60);

    return diffInHours > 2;
  }
}

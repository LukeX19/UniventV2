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

  // getFormattedStartTime(): string {
  //   const raw = this.event.startTime;
  //   const iso = raw.endsWith('Z') ? raw : raw + 'Z';
  //   const userTimeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;

  //   const startTime = new Date(iso).toLocaleString("ro-RO", {
  //     day: "2-digit",
  //     month: "2-digit",
  //     year: "numeric",
  //     hour: "2-digit",
  //     minute: "2-digit",
  //     hour12: false,
  //     timeZone: userTimeZone
  //   });
  
  //   return `Starts ${startTime}`;
  // }

  getFormattedDateParts(dateString: string) {
    const iso = dateString.endsWith('Z') ? dateString : dateString + 'Z';
    const userTimeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;

    const date = new Date(iso);

    const options = { timeZone: userTimeZone };
  
    const day = date.toLocaleDateString("ro-RO", {
      day: '2-digit',
      ...options
    });
    const month = date.toLocaleDateString("ro-RO", {
      month: 'short',
      ...options
    });
    const year = date.toLocaleDateString("ro-RO", {
      year: 'numeric',
      ...options
    });
    const time = date.toLocaleTimeString("ro-RO", {
      hour: '2-digit',
      minute: '2-digit',
      hour12: false,
      ...options
    });
  
    return { day, month, year, time };
  }

  getFormattedPostedDate(): string {
    const rawCreatedAt = this.event.createdAt;
    const isoCreatedAt = rawCreatedAt.endsWith('Z') ? rawCreatedAt : rawCreatedAt + 'Z';
    const userTimeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;

    const createdAt = new Date(isoCreatedAt).toLocaleString("ro-RO", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
      hour: "2-digit",
      minute: "2-digit",
      hour12: false,
      timeZone: userTimeZone
    });
  
    const rawUpdatedAt = this.event.updatedAt;
    const isoUpdatedAt = rawUpdatedAt.endsWith('Z') ? rawUpdatedAt : rawUpdatedAt + 'Z';

    const updatedAt = new Date(isoUpdatedAt).toLocaleString("ro-RO", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
      hour: "2-digit",
      minute: "2-digit",
      hour12: false,
      timeZone: userTimeZone
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
    this.router.navigate([`/event/${this.event.id}/edit`]);
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

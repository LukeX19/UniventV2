import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { NavbarComponent } from '../../shared/components/navbar/navbar.component';
import { EventService } from '../../core/services/event.service';
import { ActivatedRoute, Router } from '@angular/router';
import { EventAuthorResponse, EventFullResponse } from '../../shared/models/eventModel';
import { MatDividerModule } from '@angular/material/divider';
import { GoogleMap, MapMarker } from '@angular/google-maps';
import { MatDialog } from '@angular/material/dialog';
import { EventParticipantService } from '../../core/services/event-participant.service';
import { EventParticipantFullResponse } from '../../shared/models/eventParticipantModel';
import { MatButtonModule } from '@angular/material/button';
import { AuthenticationService } from '../../core/services/authentication.service';
import { SnackbarService } from '../../core/services/snackbar.service';
import { EventParticipantsDialogComponent } from '../../shared/components/event-participants-dialog/event-participants-dialog.component';

@Component({
  selector: 'app-event-details',
  standalone: true,
  imports: [
    CommonModule,
    NavbarComponent,
    MatIconModule,
    MatDividerModule,
    GoogleMap,
    MapMarker,
    MatButtonModule
  ],
  templateUrl: './event-details.component.html',
  styleUrl: './event-details.component.scss'
})
export class EventDetailsComponent {
  private route = inject(ActivatedRoute);
  private dialog = inject(MatDialog);
  private eventService = inject(EventService);
  private participantService = inject(EventParticipantService);
  private authService = inject(AuthenticationService);
  private snackbarService = inject(SnackbarService);
  private router = inject(Router);
  
  userId: string = '';
  isAuthor = false;
  isParticipant = false;

  event: EventFullResponse | null = null;
  isEventLoading = true;

  participants: EventParticipantFullResponse[] = [];
  areParticipantsLoading = true;

  ngOnInit() {
    this.authService.user$.subscribe(user => {
      if (!user) return;
      this.userId = user.id;
    });

    const resolvedEvent = this.route.snapshot.data['event'] as EventFullResponse | null;

    if (!resolvedEvent) {
      this.router.navigate(['/not-found']);
      return;
    }

    if (resolvedEvent.isCancelled) {
      this.router.navigate(['/forbidden']);
      return;
    }
    
    this.event = resolvedEvent;
    this.isAuthor = resolvedEvent.author.id === this.userId;
    this.isEventLoading = false;

    this.fetchParticipants();
  }

  fetchParticipants() {
    const eventId = this.route.snapshot.paramMap.get('id');
    if (!eventId) return;

    this.areParticipantsLoading = true;
    this.participantService.fetchEventParticipantsByEventId(eventId).subscribe({
      next: (data) => {
        this.participants = data;
        this.isParticipant = data.some(p => p.userId === this.userId);
        this.areParticipantsLoading = false;
      },
      error: (error) => {
        console.error('Error fetching participants:', error);
        this.areParticipantsLoading = false;
      }
    });
  }

  get formattedStartTime(): string {
    return new Date(this.event?.startTime || '').toLocaleString("ro-RO", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
      hour: "2-digit",
      minute: "2-digit",
      hour12: false
    });
  }

  get formattedDate(): string {
    if (!this.event) return "";
    
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
    return `${this.event?.author?.firstName || ''} ${this.event?.author?.lastName || ''}`;
  }

  getAuthorInitials(user: EventAuthorResponse | null): string {
    if (!user) return 'U';
    const firstNameInitial = user.firstName ? user.firstName.charAt(0).toUpperCase() : '';
    const lastNameInitial = user.lastName ? user.lastName.charAt(0).toUpperCase() : '';
    return `${firstNameInitial}${lastNameInitial}`;
  }

  get enrolledCount(): number {
    return this.participants.length;
  }

  get isEventFull(): boolean {
    if (!this.event) return false;
    return this.enrolledCount >= this.event.maximumParticipants;
  }

  get canJoinOrLeave(): boolean {
    if (!this.event) return false;
  
    const now = new Date();
    const eventStart = new Date(this.event.startTime);
  
    const diffInMs = eventStart.getTime() - now.getTime();
    const diffInHours = diffInMs / (1000 * 60 * 60);
  
    return diffInHours > 2;
  }
  

  openParticipantsDialog(): void {
    this.dialog.open(EventParticipantsDialogComponent, {
      data: {
        title: 'Event Participants',
        participants: this.participants,
        buttonText: 'Close'
      },
      width: '500px'
    });
  }
  
  joinEvent() {
    if (!this.event) return;

    this.participantService.createEventParticipant(this.event.id).subscribe({
      next: () => {
        this.snackbarService.success("Joined event successfully.");
        this.fetchParticipants();
      },
      error: (error) => {
        console.error("Error joining event:", error);
        this.snackbarService.error("Something went wrong. Please try again later.");
      }
    });
  }

  leaveEvent() {
    if (!this.event) return;

    this.participantService.deleteEventParticipant(this.event.id).subscribe({
      next: () => {
        this.snackbarService.success("Left event successfully.");
        this.fetchParticipants();
      },
      error: (error) => {
        console.error("Error leaving event:", error);
        this.snackbarService.error("Something went wrong. Please try again later.");
      }
    });
  }
}

import { CommonModule, Location } from '@angular/common';
import { Component, inject } from '@angular/core';
import { NavbarComponent } from '../../shared/components/navbar/navbar.component';
import { ActivatedRoute, Router } from '@angular/router';
import { EventFullResponse } from '../../shared/models/eventModel';
import { AuthenticationService } from '../../core/services/authentication.service';
import { FeedbackService } from '../../core/services/feedback.service';
import { EventParticipantService } from '../../core/services/event-participant.service';
import { MatIconModule } from '@angular/material/icon';
import { EventParticipantFullResponse } from '../../shared/models/eventParticipantModel';
import { TokenService } from '../../core/services/token.service';
import { FormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { CustomButtonComponent } from '../../shared/components/custom-button/custom-button.component';
import { SnackbarService } from '../../core/services/snackbar.service';

@Component({
  selector: 'app-feedback',
  standalone: true,
  imports: [
    CommonModule,
    NavbarComponent,
    MatIconModule,
    FormsModule,
    MatSelectModule,
    CustomButtonComponent
  ],
  templateUrl: './feedback.component.html',
  styleUrl: './feedback.component.scss'
})
export class FeedbackComponent {
  private authService = inject(AuthenticationService);
  private tokenService = inject(TokenService);
  private eventParticipantService = inject(EventParticipantService);
  private feedbackService = inject(FeedbackService);
  private snackbarService = inject(SnackbarService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private location = inject(Location);

  event: EventFullResponse | null = null;
  isEventLoading = true;

  participants: EventParticipantFullResponse[] = [];
  areParticipantsLoading = true;

  ratings: { [userId: string]: number | null } = {};

  isSubmitting = false;

  ngOnInit() {
    const resolvedEvent = this.route.snapshot.data['event'] as EventFullResponse | null;
  
    if (!resolvedEvent) {
      this.router.navigate(['/not-found']);
      return;
    }

    const now = new Date();
    const eventStart = new Date(resolvedEvent.startTime);

    // Check if event is cancelled or did not happen yet
    if (resolvedEvent.isCancelled || eventStart > now) {
      this.router.navigate(['/forbidden']);
      return;
    }
      
    // Check if user already gave feedback
    this.eventParticipantService.hasUserProvidedFeedback(resolvedEvent.id).subscribe({
      next: (data) => {
        if (data.hasCompletedFeedback) {
          this.router.navigate(['/forbidden']);
          return;
        }

        this.event = resolvedEvent;
        this.isEventLoading = false;
      },
      error: () => {
        this.router.navigate(['/forbidden']);
      }
    });

    this.fetchParticipants();
  }

  fetchParticipants() {
    const eventId = this.route.snapshot.paramMap.get('id');
    if (!eventId) return;

    this.areParticipantsLoading = true;
    const token = localStorage.getItem('uniapi-token');
    const currentUserId = this.tokenService.getUserId(token ?? '');

    this.eventParticipantService.fetchEventParticipantsByEventId(eventId).subscribe({
      next: (data) => {
        // Remove current user from participant list
        const filteredParticipants = data.filter(p => p.userId !== currentUserId);

        // Push author to the list
        const author = this.event!.author;
        const authorAsParticipant = {
          eventId: this.event!.id,
          userId: author.id,
          firstName: author.firstName,
          lastName: author.lastName,
          pictureUrl: author.pictureUrl ?? null,
          rating: author.rating,
          hasCompletedFeedback: false
        };
        filteredParticipants.push(authorAsParticipant);
        
        this.participants = filteredParticipants;
        this.areParticipantsLoading = false;
      },
      error: (error) => {
        console.error('Error fetching participants:', error);
        this.areParticipantsLoading = false;
      }
    });
  }

  get formattedStartTime(): string {
    if (!this.event) return '';

    const raw = this.event.startTime;
    const iso = raw.endsWith('Z') ? raw : raw + 'Z';
    const userTimeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;

    return new Date(iso).toLocaleString("ro-RO", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
      hour: "2-digit",
      minute: "2-digit",
      hour12: false,
      timeZone: userTimeZone
    });
  }

  getInitials(firstName: string, lastName: string): string {
    const firstNameInitial = firstName?.charAt(0).toUpperCase() || '';
    const lastNameInitial = lastName?.charAt(0).toUpperCase() || '';
    return `${firstNameInitial}${lastNameInitial}`;
  }

  onRatingChange(userId: string, value: number | null) {
    if (value === null) {
      delete this.ratings[userId];
    } else {
      this.ratings[userId] = value;
    }
  }

  isRatingsEmpty(): boolean {
    return Object.keys(this.ratings).length === 0;
  }

  onUserClick(id: string) {
    this.router.navigate([`/profile/${id}`]);
  }

  cancel() {
    this.location.back();
  }

  submitFeedback() {
    if (!this.event || this.isSubmitting) return;

    this.isSubmitting = true;

    const recipients = Object.entries(this.ratings).map(([userId, rating]) => ({
      userId,
      rating: rating!
    }));

    const feedbackData = { recipients };

    this.feedbackService.submitMultipleFeedbacks(this.event.id, feedbackData).subscribe({
      next: () => {
        this.snackbarService.success('Feedback sent successfully.');
        this.authService.fetchUser();
        this.router.navigate([`/browse`]);
      },
      error: (error) => {
        console.error('Failed to submit feedback:', error);
        this.snackbarService.error("Something went wrong. Please try again later.");
        this.isSubmitting = false;
      }
    });
  }
}

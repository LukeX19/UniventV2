import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { NavbarComponent } from '../../shared/components/navbar/navbar.component';
import { ActivatedRoute, Router } from '@angular/router';
import { EventFullResponse } from '../../shared/models/eventModel';
import { AuthenticationService } from '../../core/services/authentication.service';
import { EventService } from '../../core/services/event.service';
import { FeedbackService } from '../../core/services/feedback.service';
import { EventParticipantService } from '../../core/services/event-participant.service';

@Component({
  selector: 'app-feedback',
  standalone: true,
  imports: [
    CommonModule,
    NavbarComponent,
  ],
  templateUrl: './feedback.component.html',
  styleUrl: './feedback.component.scss'
})
export class FeedbackComponent {
  private authService = inject(AuthenticationService);
  private eventService = inject(EventService);
  private eventParticipantService = inject(EventParticipantService);
  private feedbackService = inject(FeedbackService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  event: EventFullResponse | null = null;
  isEventLoading = true;

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
  }
}

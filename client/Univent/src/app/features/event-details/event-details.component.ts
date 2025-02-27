import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { NavbarComponent } from '../../shared/components/navbar/navbar.component';
import { EventService } from '../../core/services/event.service';
import { ActivatedRoute } from '@angular/router';
import { EventFullResponse } from '../../shared/models/eventModel';

@Component({
  selector: 'app-event-details',
  standalone: true,
  imports: [
    CommonModule,
    MatIconModule,
    NavbarComponent
  ],
  templateUrl: './event-details.component.html',
  styleUrl: './event-details.component.scss'
})
export class EventDetailsComponent {
  private eventService = inject(EventService);
  private route = inject(ActivatedRoute);

  event: EventFullResponse | null = null;
  isLoading = true;

  ngOnInit() {
    this.fetchEvent();
  }

  fetchEvent() {
    const eventId = this.route.snapshot.paramMap.get('id');
    if (!eventId) return;

    this.isLoading = true;
    this.eventService.fetchEventById(eventId).subscribe({
      next: (data) => {
        this.event = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error("Error fetching event:", error);
        this.isLoading = false;
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

    return updatedAt > createdAt ? `Updated on ${updatedAt}` : `Created on ${createdAt}`;
  }

  getAuthorFullName(): string {
    return `${this.event?.author?.firstName || ''} ${this.event?.author?.lastName || ''}`;
  }
}

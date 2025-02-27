import { Component, inject, Input } from '@angular/core';
import { EventSummaryResponse } from '../../models/eventModel';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-event-card',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule
  ],
  templateUrl: './event-card.component.html',
  styleUrl: './event-card.component.scss'
})
export class EventCardComponent {
  private router = inject(Router);
  
  @Input() event!: EventSummaryResponse;

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
  
    return updatedAt > createdAt ? `Updated on ${updatedAt}` : `Created on ${createdAt}`;
  }
  

  getAuthorFullName(): string {
    return `${this.event.author.firstName} ${this.event.author.lastName}`;
  }

  navigateToEvent() {
    this.router.navigate([`/event/${this.event.id}`]);
  }
}

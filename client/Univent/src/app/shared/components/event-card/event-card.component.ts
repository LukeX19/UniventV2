import { Component, Input } from '@angular/core';
import { EventCardResponse } from '../../models/eventModel';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';

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
  @Input() event!: EventCardResponse;

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
    return `${this.event.authorFirstName} ${this.event.authorLastName}`;
  }

  navigateToEvent() {
    console.log(`Navigating to event: ${this.event.id}`);
    // Implement navigation logic here if needed.
  }
}

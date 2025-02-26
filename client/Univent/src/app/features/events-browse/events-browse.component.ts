import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from '../../shared/components/navbar/navbar.component';
import { EventCardComponent } from "../../shared/components/event-card/event-card.component";
import { EventService } from '../../core/services/event.service';
import { EventSummaryResponse } from '../../shared/models/eventModel';

@Component({
  selector: 'app-events-browse',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule,
    NavbarComponent,
    EventCardComponent
],
  templateUrl: './events-browse.component.html',
  styleUrl: './events-browse.component.scss'
})
export class EventsBrowseComponent {
  private eventService = inject(EventService);
  
  events: EventSummaryResponse[] = [];
  isLoading = true;

  ngOnInit() {
    this.fetchEvents();
  }

  fetchEvents() {
    this.isLoading = true;

    this.eventService.getAllEventsSummaries().subscribe({
      next: (data) => {
        this.events = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error("Error fetching events:", error);
        this.isLoading = false;
      }
    });
  }
}

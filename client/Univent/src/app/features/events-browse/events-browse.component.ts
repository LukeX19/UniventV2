import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from '../../shared/components/navbar/navbar.component';
import { EventCardComponent } from "../../shared/components/event-card/event-card.component";
import { EventService } from '../../core/services/event.service';
import { EventSummaryResponse } from '../../shared/models/eventModel';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { PaginationRequest } from '../../shared/models/paginationModel';

@Component({
  selector: 'app-events-browse',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule,
    NavbarComponent,
    EventCardComponent,
    MatPaginatorModule
],
  templateUrl: './events-browse.component.html',
  styleUrl: './events-browse.component.scss'
})
export class EventsBrowseComponent {
  private eventService = inject(EventService);
  
  events: EventSummaryResponse[] = [];
  isLoading = true;

  pagination: PaginationRequest = { pageIndex: 1, pageSize: 10 };
  totalEvents = 0;
  totalPages = 1;

  ngOnInit() {
    this.fetchEvents();
  }

  fetchEvents() {
    this.isLoading = true;

    this.eventService.fetchAllEventsSummaries(this.pagination).subscribe({
      next: (data) => {
        this.events = data.elements;
        this.totalEvents = data.resultsCount;
        this.totalPages = data.totalPages;
        this.pagination.pageIndex = data.pageIndex;
        
        this.isLoading = false;
      },
      error: (error) => {
        console.error("Error fetching events:", error);
        this.isLoading = false;
      }
    });
  }

  onPageChange(event: PageEvent) {
    if (event.pageSize !== this.pagination.pageSize) {
      this.pagination.pageIndex = 1;
    } else {
      this.pagination.pageIndex = event.pageIndex + 1;
    }

    this.pagination.pageSize = event.pageSize;
    this.fetchEvents();
  }
}

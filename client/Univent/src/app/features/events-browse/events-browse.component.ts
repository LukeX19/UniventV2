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
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { EventTypeService } from '../../core/services/event-type.service';
import { MatDrawer, MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { AiAssistantWidgetComponent } from "../../shared/components/ai-assistant-widget/ai-assistant-widget.component";

@Component({
  selector: 'app-events-browse',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule,
    NavbarComponent,
    EventCardComponent,
    MatPaginatorModule,
    FormsModule,
    MatIconModule,
    MatSidenavModule,
    MatListModule,
    AiAssistantWidgetComponent
],
  templateUrl: './events-browse.component.html',
  styleUrl: './events-browse.component.scss'
})
export class EventsBrowseComponent {
  private eventService = inject(EventService);
  private eventTypeService = inject(EventTypeService);
  
  events: EventSummaryResponse[] = [];
  isLoading = true;

  pagination: PaginationRequest = { pageIndex: 1, pageSize: 10 };
  totalEvents = 0;
  totalPages = 1;

  searchQuery: string = '';

  eventTypes: { id: string, name: string }[] = [];
  selectedEventTypeIds: (string | null)[] = [];

  ngOnInit() {
    this.fetchEvents();
    this.fetchEventTypes();
  }

  fetchEvents() {
    this.isLoading = true;

    const typeFilters = this.selectedEventTypeIds.includes(null) ? [] : this.selectedEventTypeIds;

    this.eventService.fetchAllEventsSummaries(this.pagination,
      this.searchQuery, typeFilters as string[]).subscribe({
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

  fetchEventTypes() {
    this.eventTypeService.fetchActiveEventTypes().subscribe({
      next: (data) => {
        this.eventTypes = data;
      },
      error: (error) => console.error("Error fetching event types:", error)
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

  onSearch() {
    this.pagination.pageIndex = 1;
    this.fetchEvents();
  }

  onFilterChange(drawer: MatDrawer) {
    this.pagination.pageIndex = 1;

    if (this.selectedEventTypeIds.includes(null)) {
      this.selectedEventTypeIds = [];
    }

    this.fetchEvents();
  }
}

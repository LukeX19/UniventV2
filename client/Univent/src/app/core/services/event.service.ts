import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { EventFullResponse, CreateEventRequest, EventSummaryResponse, UpdateEventRequest, EventSummaryWithFeedbackStatusResponse } from '../../shared/models/eventModel';
import { Observable } from 'rxjs';
import { PaginationRequest, PaginationResponse } from '../../shared/models/paginationModel';

@Injectable({
  providedIn: 'root'
})
export class EventService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}`;

  createEvent(eventData: CreateEventRequest): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/events`, eventData, {
      headers: { 'Requires-Auth': 'true' }
    });
  }

  fetchAllEventsSummaries(pagination: PaginationRequest,
    searchQuery?: string, eventTypeIds?: string[]): Observable<PaginationResponse<EventSummaryResponse>> {
    let params = new HttpParams()
      .set('pageIndex', pagination.pageIndex.toString())
      .set('pageSize', pagination.pageSize.toString());
    
    if (searchQuery && searchQuery.trim() !== '') {
      params = params.set('search', searchQuery.trim());
    }

    if (eventTypeIds && eventTypeIds.length > 0) {
      eventTypeIds.forEach(id => {
        if (id) {
          params = params.append('types', id);
        }
      });
    }
    
    return this.http.get<PaginationResponse<EventSummaryResponse>>(`${this.apiUrl}/events/`, {
      params,
      headers: { 'Requires-Auth': 'true' }
    });
  }

  fetchCreatedEventsSummariesByUserId(userId: string, pagination: PaginationRequest): Observable<PaginationResponse<EventSummaryResponse>> {
    const params = new HttpParams()
      .set('pageIndex', pagination.pageIndex.toString())
      .set('pageSize', pagination.pageSize.toString());
    
    return this.http.get<PaginationResponse<EventSummaryResponse>>(`${this.apiUrl}/events/created-by/${userId}`, {
      params,
      headers: { 'Requires-Auth': 'true' }
    });
  }

  fetchParticipatedEventsSummariesByUserId(userId: string, pagination: PaginationRequest): Observable<PaginationResponse<EventSummaryWithFeedbackStatusResponse>> {
    const params = new HttpParams()
      .set('pageIndex', pagination.pageIndex.toString())
      .set('pageSize', pagination.pageSize.toString());
    
    return this.http.get<PaginationResponse<EventSummaryWithFeedbackStatusResponse>>(`${this.apiUrl}/events/participated-by/${userId}`, {
      params,
      headers: { 'Requires-Auth': 'true' }
    });
  }

  fetchEventById(id: string): Observable<EventFullResponse> {
    return this.http.get<EventFullResponse>(`${this.apiUrl}/events/${id}`, {
      headers: { 'Requires-Auth': 'true' }
    });
  }

  updateEvent(id: string, eventData: UpdateEventRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/events/${id}`, eventData, {
      headers: { 'Requires-Auth': 'true' }
    });
  }

  cancelEvent(id: string): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/events/${id}/cancel`, null, {
      headers: { 'Requires-Auth': 'true' }
    });
  }
}

import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { EventFullResponse, EventRequest, EventSummaryResponse } from '../../shared/models/eventModel';
import { Observable } from 'rxjs';
import { PaginationRequest, PaginationResponse } from '../../shared/models/paginationModel';

@Injectable({
  providedIn: 'root'
})
export class EventService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}`;

  createEvent(eventData: EventRequest): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/events`, eventData, {
      headers: { 'Requires-Auth': 'true' }
    });
  }

  fetchAllEventsSummaries(pagination: PaginationRequest): Observable<PaginationResponse<EventSummaryResponse>> {
    const params = new HttpParams()
      .set('pageIndex', pagination.pageIndex.toString())
      .set('pageSize', pagination.pageSize.toString());
    
    return this.http.get<PaginationResponse<EventSummaryResponse>>(`${this.apiUrl}/events/`, {
      params,
      headers: { 'Requires-Auth': 'true' }
    });
  }

  fetchEventById(id: string): Observable<EventFullResponse> {
    return this.http.get<EventFullResponse>(`${this.apiUrl}/events/${id}`, {
      headers: { 'Requires-Auth': 'true' }
    });
  }
}

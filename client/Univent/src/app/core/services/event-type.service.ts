import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { EventTypeRequest, EventTypeResponse } from '../../shared/models/eventTypeModel';
import { Observable } from 'rxjs';
import { PaginationRequest, PaginationResponse } from '../../shared/models/paginationModel';

@Injectable({
  providedIn: 'root'
})
export class EventTypeService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}`;

  createEventType(eventTypeData: EventTypeRequest): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/event-types`, eventTypeData, {
      headers: { 'Requires-Auth': 'true' }
    });
  }

  fetchActiveEventTypes(): Observable<EventTypeResponse[]> {
    return this.http.get<EventTypeResponse[]>(`${this.apiUrl}/event-types/active`, {
      headers: { 'Requires-Auth': 'true' }
    });
  }

  fetchAllEventTypes(pagination: PaginationRequest): Observable<PaginationResponse<EventTypeResponse>> {
    let params = new HttpParams()
      .set('pageIndex', pagination.pageIndex.toString())
      .set('pageSize', pagination.pageSize.toString());
    
    return this.http.get<PaginationResponse<EventTypeResponse>>(`${this.apiUrl}/event-types/`, {
      params,
      headers: { 'Requires-Auth': 'true' }
    });
  }

  updateEventType(id: string, eventTypeData: EventTypeRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/event-types/${id}`, eventTypeData, {
      headers: { 'Requires-Auth': 'true' }
    });
  }

  enableEventType(id: string): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/event-types/${id}/enable`, {}, {
      headers: { 'Requires-Auth': 'true' }
    });
  }
  
  disableEventType(id: string): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/event-types/${id}/disable`, {}, {
      headers: { 'Requires-Auth': 'true' }
    });
  }
}

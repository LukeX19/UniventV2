import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { EventRequest, EventSummaryResponse } from '../../shared/models/eventModel';
import { Observable } from 'rxjs';

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

  getAllEventsSummaries(): Observable<EventSummaryResponse[]> {
    return this.http.get<EventSummaryResponse[]>(`${this.apiUrl}/events/`, {
      headers: { 'Requires-Auth': 'true' }
    });
  }
}

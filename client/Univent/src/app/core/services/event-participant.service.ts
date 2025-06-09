import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { EventParticipantFeedbackStatus, EventParticipantFullResponse } from '../../shared/models/eventParticipantModel';

@Injectable({
  providedIn: 'root'
})
export class EventParticipantService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}`;

  createEventParticipant(eventId: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/event-participants/${eventId}`, null, {
      headers: { 'Requires-Auth': 'true' }
    });
  }

  fetchEventParticipantsByEventId(eventId: string): Observable<EventParticipantFullResponse[]> {
    return this.http.get<EventParticipantFullResponse[]>(`${this.apiUrl}/event-participants/${eventId}/participants`, {
      headers: { 'Requires-Auth': 'true' }
    });
  }

  hasUserProvidedFeedback(eventId: string): Observable<EventParticipantFeedbackStatus> {
    return this.http.get<EventParticipantFeedbackStatus>(`${this.apiUrl}/event-participants/${eventId}/participant/feedback-status`, {
      headers: { 'Requires-Auth': 'true' }
    });
  }

  deleteEventParticipant(eventId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/event-participants/${eventId}`, {
      headers: { 'Requires-Auth': 'true' }
    });
  }
}

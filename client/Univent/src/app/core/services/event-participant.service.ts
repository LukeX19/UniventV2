import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { EventParticipantFullResponse } from '../../shared/models/eventParticipantModel';

@Injectable({
  providedIn: 'root'
})
export class EventParticipantService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}`;

  fetchEventParticipantsByEventId(eventId: string): Observable<EventParticipantFullResponse[]> {
    return this.http.get<EventParticipantFullResponse[]>(`${this.apiUrl}/event-participants/${eventId}/participants`, {
      headers: { 'Requires-Auth': 'true' }
    });
  }
}

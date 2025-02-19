import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { EventTypeResponse } from '../../shared/models/eventTypeModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventTypeService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}`;

  fetchActiveEventTypes(): Observable<EventTypeResponse[]> {
    return this.http.get<EventTypeResponse[]>(`${this.apiUrl}/event-types/active`, {
      headers: { 'Requires-Auth': 'true' }
    });
  }
}

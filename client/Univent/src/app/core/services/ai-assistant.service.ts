import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AiAssistantService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}`;

  askForInterestsBasedRecommendations(userDescription: string): Observable<string> {
    const headers = {
      'Content-Type': 'application/json',
      'Requires-Auth': 'true'
    };

    return this.http.post<string>(`${this.apiUrl}/ai-assistant/events/interests/recommend`, JSON.stringify(userDescription), {
      headers,
      responseType: 'text' as 'json'
    });
  }

  askForLocationBasedRecommendations(locationDescription: string): Observable<string> {
    const headers = {
      'Content-Type': 'application/json',
      'Requires-Auth': 'true'
    };

    return this.http.post<string>(`${this.apiUrl}/ai-assistant/events/location/recommend`, JSON.stringify(locationDescription), {
      headers,
      responseType: 'text' as 'json'
    });
  }

  askForTimeBasedRecommendations(timePreference: string): Observable<string> {
    const headers = {
      'Content-Type': 'application/json',
      'Requires-Auth': 'true'
    };

    return this.http.post<string>(`${this.apiUrl}/ai-assistant/events/time/recommend`, JSON.stringify(timePreference), {
      headers,
      responseType: 'text' as 'json'
    });
  }

  askForWeatherBasedRecommendations(): Observable<string> {
    const headers = {
      'Content-Type': 'application/json',
      'Requires-Auth': 'true'
    };

    return this.http.post<string>(`${this.apiUrl}/ai-assistant/events/weather/recommend`, {}, {
      headers,
      responseType: 'text' as 'json'
    });
  }
}

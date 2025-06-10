import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { CreateMultipleFeedbacksDto } from '../../shared/models/feedbackModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FeedbackService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}`;

  submitMultipleFeedbacks(eventId: string, feedbackData: CreateMultipleFeedbacksDto): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/feedbacks/event/${eventId}/submit`, feedbackData, {
      headers: { 'Requires-Auth': 'true' }
    });
  }
}

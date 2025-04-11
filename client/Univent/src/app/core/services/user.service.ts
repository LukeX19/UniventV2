import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { UniversityResponse } from '../../shared/models/universityModel';
import { UserProfileResponse } from '../../shared/models/userModel';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}`;

  fetchUserProfileById(id: string): Observable<UserProfileResponse> {
    return this.http.get<UserProfileResponse>(`${this.apiUrl}/users/profile/${id}`, {
      headers: { 'Requires-Auth': 'true' }
    });
  }
}

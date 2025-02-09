import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { UniversityResponse } from '../../shared/models/universityModel';

@Injectable({
  providedIn: 'root'
})
export class UniversityService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}`;
  
  searchUniversity(query: string): Observable<UniversityResponse[]> {
    return this.http.get<UniversityResponse[]>(`${this.apiUrl}/universities/search?input=${query}`);
  }
}

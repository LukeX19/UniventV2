import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { UniversityResponse, UniversityRequest } from '../../shared/models/universityModel';
import { PaginationRequest, PaginationResponse } from '../../shared/models/paginationModel';

@Injectable({
  providedIn: 'root'
})
export class UniversityService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}`;

  createUniversity(universityData: UniversityRequest): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/universities`, universityData, {
      headers: { 'Requires-Auth': 'true' }
    });
  }

  fetchAllUniversities(pagination: PaginationRequest): Observable<PaginationResponse<UniversityResponse>> {
    let params = new HttpParams()
      .set('pageIndex', pagination.pageIndex.toString())
      .set('pageSize', pagination.pageSize.toString());
    
    return this.http.get<PaginationResponse<UniversityResponse>>(`${this.apiUrl}/universities/`, {
      params,
      headers: { 'Requires-Auth': 'true' }
    });
  }

  updateUniversity(id: string, universityData: UniversityRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/universities/${id}`, universityData, {
      headers: { 'Requires-Auth': 'true' }
    });
  }

  deleteUniversity(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/universities/${id}`, {
      headers: { 'Requires-Auth': 'true' }
    });
  }
  
  searchUniversity(query: string): Observable<UniversityResponse[]> {
    return this.http.get<UniversityResponse[]>(`${this.apiUrl}/universities/search?input=${query}`);
  }
}

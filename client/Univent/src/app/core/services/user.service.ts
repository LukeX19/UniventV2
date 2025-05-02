import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { UserManagementResponse, UserProfileResponse } from '../../shared/models/userModel';
import { PaginationRequest, PaginationResponse } from '../../shared/models/paginationModel';

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

  fetchAllUsers(pagination: PaginationRequest): Observable<PaginationResponse<UserManagementResponse>> {
    let params = new HttpParams()
      .set('pageIndex', pagination.pageIndex.toString())
      .set('pageSize', pagination.pageSize.toString());
    
    return this.http.get<PaginationResponse<UserManagementResponse>>(`${this.apiUrl}/users`, {
      params,
      headers: { 'Requires-Auth': 'true' }
    });
  }
}

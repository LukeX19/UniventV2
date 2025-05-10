import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { UpdateUserProfileRequest, UserManagementResponse, UserProfileResponse } from '../../shared/models/userModel';
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

  approveUser(id: string): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/users/${id}/approve`, null, {
      headers: { 'Requires-Auth': 'true' }
    });
  }

  banUser(id: string): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/users/${id}/ban`, null, {
      headers: { 'Requires-Auth': 'true' }
    });
  }

  updateUserProfile(userProfileData: UpdateUserProfileRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/users/profile`, userProfileData, {
      headers: { 'Requires-Auth': 'true' }
    });
  }
}

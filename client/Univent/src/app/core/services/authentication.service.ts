import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { LoginRequest, LoginResponse, RegisterRequest } from '../../shared/models/authenticationModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}`;

  login(loginInfo: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/authentication/login`, loginInfo);
  }

  register(registerData: RegisterRequest): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/authentication/register`, registerData);
  }
}

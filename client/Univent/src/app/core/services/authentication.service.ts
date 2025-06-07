import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { LoginRequest, LoginResponse, RegisterRequest } from '../../shared/models/authenticationModel';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { UserResponse } from '../../shared/models/userModel';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}`;
  private tokenService = inject(TokenService);

  private userSubject = new BehaviorSubject<UserResponse | null>(null);
  user$: Observable<UserResponse | null> = this.userSubject.asObservable();

  constructor() {
    this.restoreUserSession();
  }

  login(loginInfo: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/authentication/login`, loginInfo).pipe(
      tap((response: LoginResponse) => {
        localStorage.setItem('uniapi-token', response.token);
        this.fetchUser();
      })
    );
  }

  register(registerData: RegisterRequest): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/authentication/register`, registerData);
  }

  fetchUser(): void {
    const token = localStorage.getItem('uniapi-token');
    if (!token) return;

    const role = this.tokenService.getUserRole(token);
    if (role === 1) {
      this.http.get<UserResponse>(`${this.apiUrl}/users/current`, {
        headers: { 'Requires-Auth': 'true' }
      }).subscribe({
        next: (user) => {
          this.userSubject.next(user);
        },
        error: (error) => {
          console.error("Failed to fetch user:", error);
          this.userSubject.next(null);
        }
      });
    }
  }

  logout(): void {
    localStorage.removeItem('uniapi-token');
    this.userSubject.next(null);
  }

  private restoreUserSession(): void {
    const token = localStorage.getItem('uniapi-token');

    if (token) {
      this.fetchUser();
    }
  }
}

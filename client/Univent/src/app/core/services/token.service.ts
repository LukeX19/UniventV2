import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

interface DecodedToken {
  uid: string;
  role: number;
}

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  decodeToken(token: string): DecodedToken | null {
    try {
      return jwtDecode<DecodedToken>(token);
    } catch (error) {
      console.error('Failed to decode token', error);
      return null;
    }
  }

  getUserId(token: string): string | null {
    const decodedToken = this.decodeToken(token);
    return decodedToken?.uid ?? null;
  }

  getUserRole(token: string): number | null {
    const decodedToken = this.decodeToken(token);
    return decodedToken?.role ?? null;
  }
}

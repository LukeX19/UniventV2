import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { TokenService } from '../services/token.service';

export const userGuard: CanActivateFn = () => {
  const tokenService = inject(TokenService);
  const router = inject(Router);

  const token = localStorage.getItem('uniapi-token');
  if (!token) {
    router.navigate(['/forbidden']);
    return false;
  }

  const role = tokenService.getUserRole(token);
  if (role === 1) {
    return true;
  } else {
    router.navigate(['/forbidden']);
    return false;
  }
};

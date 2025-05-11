import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { TokenService } from '../services/token.service';

export const ownerGuard: CanActivateFn = (route, state) => {
  const tokenService = inject(TokenService);
  const router = inject(Router);

  const token = localStorage.getItem('uniapi-token');
  if (!token) {
    router.navigate(['/forbidden']);
    return false;
  }

  const loggedInUserId = tokenService.getUserId(token);
  const paramUserId = route.paramMap.get('id');

  if (paramUserId && loggedInUserId === paramUserId) {
    return true;
  }

  router.navigate(['/forbidden']);
  return false;
};

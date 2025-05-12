import { ResolveFn } from '@angular/router';
import { UserProfileResponse } from '../../shared/models/userModel';
import { inject } from '@angular/core';
import { UserService } from '../services/user.service';
import { catchError, of } from 'rxjs';

export const userResolver: ResolveFn<UserProfileResponse | null> = (route, state) => {
  const userService = inject(UserService);
  const userId = route.paramMap.get('id');
  
  if (!userId) return of(null);

  return userService.fetchUserProfileById(userId).pipe(
    catchError(() => of(null))
  );
};

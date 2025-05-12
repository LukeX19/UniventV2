import { ResolveFn } from '@angular/router';
import { EventFullResponse } from '../../shared/models/eventModel';
import { inject } from '@angular/core';
import { EventService } from '../services/event.service';
import { catchError, of } from 'rxjs';

export const eventResolver: ResolveFn<EventFullResponse | null> = (route, state) => {
  const eventService = inject(EventService);
  const eventId = route.paramMap.get('id');

  if (!eventId) return of(null);

  return eventService.fetchEventById(eventId).pipe(
    catchError(() => of(null))
  );
};

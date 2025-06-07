import { Routes } from '@angular/router';
import { LoginComponent } from './features/authentication/login/login.component';
import { RegisterComponent } from './features/authentication/register/register.component';
import { EventCreateComponent } from './features/event-create/event-create.component';
import { EventsBrowseComponent } from './features/events-browse/events-browse.component';
import { EventDetailsComponent } from './features/event-details/event-details.component';
import { ProfileComponent } from './features/profile/profile.component';
import { EventUpdateComponent } from './features/event-update/event-update.component';
import { AdminDashboardComponent } from './features/admin-dashboard/admin-dashboard.component';
import { userGuard } from './core/guards/user.guard';
import { adminGuard } from './core/guards/admin.guard';
import { ForbiddenComponent } from './features/errors/forbidden/forbidden.component';
import { NotFoundComponent } from './features/errors/not-found/not-found.component';
import { LandingComponent } from './features/landing/landing.component';
import { ProfileUpdateComponent } from './features/profile-update/profile-update.component';
import { eventResolver } from './core/resolvers/event.resolver';
import { userResolver } from './core/resolvers/user.resolver';

export const routes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  { path: 'host', component: EventCreateComponent, canActivate: [userGuard] },
  { path: 'browse', component: EventsBrowseComponent, canActivate: [userGuard] },
  { path: 'event/:id', component: EventDetailsComponent, canActivate: [userGuard], resolve: { event: eventResolver } },
  { path: 'event/:id/edit', component: EventUpdateComponent, canActivate: [userGuard], resolve: { event: eventResolver } },
  { path: 'profile/:id', component: ProfileComponent, canActivate: [userGuard], resolve: { user: userResolver } },
  { path: 'profile/:id/edit', component: ProfileUpdateComponent, canActivate: [userGuard], resolve: { user: userResolver } },
  
  { path: 'admin/dashboard', component: AdminDashboardComponent, canActivate: [adminGuard] },

  { path: 'forbidden', component: ForbiddenComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: '**', redirectTo: 'not-found', pathMatch: 'full' }
];

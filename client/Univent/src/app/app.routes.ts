import { Routes } from '@angular/router';
import { LoginComponent } from './features/authentication/login/login.component';
import { RegisterComponent } from './features/authentication/register/register.component';
import { HomeComponent } from './features/home/home.component';
import { EventCreateComponent } from './features/event-create/event-create.component';
import { EventsBrowseComponent } from './features/events-browse/events-browse.component';
import { EventDetailsComponent } from './features/event-details/event-details.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'home', component: HomeComponent },
  { path: 'host', component: EventCreateComponent },
  { path: 'browse', component: EventsBrowseComponent},
  { path: 'event/:id', component: EventDetailsComponent }
];

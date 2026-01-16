import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { Routes } from '@angular/router';
import { AppComponent } from './app/app.component';

// Import components
import { ClubListComponent } from './app/components/club-list/club-list.component';
import { ClubFormComponent } from './app/components/club-form/club-form.component';
import { ClubDetailComponent } from './app/components/club-detail/club-detail.component';

// Define routes here
const routes: Routes = [
  { path: '', redirectTo: '/clubs', pathMatch: 'full' },
  { path: 'clubs', component: ClubListComponent },
  { path: 'clubs/add', component: ClubFormComponent },
  { path: 'clubs/edit/:id', component: ClubFormComponent },
  { path: 'clubs/:id', component: ClubDetailComponent }
];

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes),
    provideHttpClient()
  ]
}).catch(err => console.error(err));

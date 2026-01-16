import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClubListComponent } from './components/club-list/club-list.component';
import { ClubFormComponent } from './components/club-form/club-form.component';
import { ClubDetailComponent } from './components/club-detail/club-detail.component';

const routes: Routes = [
  { path: '', redirectTo: '/clubs', pathMatch: 'full' },
  { path: 'clubs', component: ClubListComponent },
  { path: 'clubs/add', component: ClubFormComponent },
  { path: 'clubs/edit/:id', component: ClubFormComponent },
  { path: 'clubs/:id', component: ClubDetailComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

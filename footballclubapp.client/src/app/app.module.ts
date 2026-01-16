import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

// Import components
import { ClubListComponent } from './components/club-list/club-list.component';
import { ClubFormComponent } from './components/club-form/club-form.component';
import { ClubDetailComponent } from './components/club-detail/club-detail.component';

@NgModule({
  declarations: [
    AppComponent,
    ClubListComponent,
    ClubFormComponent,
    ClubDetailComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './modules/home/home.component';
import { BookingSearchComponent } from './modules/booking-search/booking-search.component';
import { BookingCalendarComponent } from './modules/booking-calendar/booking-calendar.component';
import { FullCalendarModule } from '@fullcalendar/angular';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AvailableHoursModalComponent } from './modules/modal/available-hours-modal/available-hours-modal.component';
import { SubmitModalComponent } from './modules/modal/submit-modal/submit-modal.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    BookingSearchComponent,
    BookingCalendarComponent,
    AvailableHoursModalComponent,
    SubmitModalComponent
    
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FullCalendarModule,
    NgbModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

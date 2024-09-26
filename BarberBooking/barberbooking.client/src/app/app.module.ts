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
import { FormsModule } from '@angular/forms';
import { CreateReservationComponent } from './modules/create-reservation/create-reservation.component';
import { NavigationComponent } from './modules/navigation/navigation.component';
import { ListReservationsComponent } from './modules/list-reservations/list-reservations.component';
import { SignUpComponent } from './modules/sign-up/sign-up.component';
import { NotificationComponent } from './modules/notification/notification.component';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    BookingSearchComponent,
    BookingCalendarComponent,
    AvailableHoursModalComponent,
    SubmitModalComponent,
    CreateReservationComponent,
    NavigationComponent,
    ListReservationsComponent,
    SignUpComponent,
    NotificationComponent
    
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FullCalendarModule,
    NgbModule,
    FormsModule,
    BrowserAnimationsModule, // Required for animations
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

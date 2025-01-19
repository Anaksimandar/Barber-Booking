import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
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
import { DateEmiterService } from './services/date-emiter.service';
import { EditReservationModalComponent } from 'src/app/modules/modal/edit-reservation-modal/edit-reservation-modal.component';
import { ReactiveFormsModule } from '@angular/forms';
import { LogInComponent } from './modules/log-in/log-in.component';
import { AccountService } from './services/account.service';
import { AuthInterceptor } from './modules/rest/auth-interceptor';
import { ChangePasswordComponent } from './modules/change-password/change-password.component';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { ResetPasswordComponent } from './modules/reset-password/reset-password.component';
import { ForgotPasswordComponent } from './modules/forgot-password/forgot-password.component';
import { MatIconModule } from '@angular/material/icon';

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
    NotificationComponent,
    EditReservationModalComponent,
    LogInComponent,
    ChangePasswordComponent,
    ResetPasswordComponent,
    ForgotPasswordComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FullCalendarModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule, // Required for animations
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatSelectModule,
    MatIconModule,
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
    })
  ],
  providers: [DateEmiterService, AccountService, {provide:HTTP_INTERCEPTORS, useClass:AuthInterceptor,multi:true}],
  bootstrap: [AppComponent]
})
export class AppModule { }

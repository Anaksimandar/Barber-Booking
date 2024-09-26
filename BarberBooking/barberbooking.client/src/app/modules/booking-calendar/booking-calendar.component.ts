import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output, inject } from '@angular/core';
import { CalendarOptions } from '@fullcalendar/core'
import dayGridPlugin from '@fullcalendar/daygrid'
import interactionPlugin from '@fullcalendar/interaction'
import { Reservation } from '../../../models/reservation.model';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AvailableHoursModalComponent } from '../modal/available-hours-modal/available-hours-modal.component';
import { SubmitModalComponent } from 'src/app/modules/modal/submit-modal/submit-modal.component';
interface StartEndDate {
  startDate: Date,
  endDate: Date
}

@Component({
  selector: 'app-booking-calendar',
  templateUrl: './booking-calendar.component.html',
  styleUrls: ['./booking-calendar.component.css']
})


export class BookingCalendarComponent implements OnInit {
  @Output() dateSelected = new EventEmitter<Date>();
  private readonly minHour: number = 10;
  private readonly maxHour: number = 15;
  private readonly gapMinutes: number = 30; // it will be changed to booking.service.length
  private reservations: Reservation[] = [];
  private availableHoursModalRef!: NgbModalRef;
  private modalService = inject(NgbModal);
  private submitModalRef!: NgbModalRef;
  private choosenBookingDate!: Date;
  constructor(private http:HttpClient) {

  }
  calendarOptions: CalendarOptions = {
    plugins: [dayGridPlugin, interactionPlugin],
    initialView: 'dayGridMonth',
    dateClick: this.handleDateClick.bind(this),
    events: [],
  }

  loadBookings(): void {
    this.http.get<Reservation[]>("https://localhost:7030/api/reservation").subscribe(
      result => {
        this.calendarOptions.events = result.map(booking => ({
          title: booking.userId?.toString(),
          start: new Date(booking.createdAt!),
          allDay: false
        })),
        this.reservations = result;
      },
      error => alert(error)
    )
  }

  //openAvailableHoursModal(date: Date, bookedHours: Date[]): void {
  //  const availableHours = this.computeAvailableHours(bookedHours);

  //  const modalRef = this.modalService.open(AvailableHoursModalComponent);
  //  modalRef.componentInstance.date = date;
  //  modalRef.componentInstance.availableHours = availableHours;
  //}

  

  computeAvailableHours(bookedHours: StartEndDate[]): string[] {
    const availableAppointment: string[] = [];

    for (let hour = this.minHour; hour < this.maxHour; hour++) {
      for (let minutes = 0; minutes < 60; minutes += this.gapMinutes) {
        let isAvailable = true;
        // if there is no booked hours we will just all avaiable hours/minutes to list
        if (bookedHours.length != 0) {
          const time = new Date(bookedHours[0].startDate);
          time.setHours(hour, minutes, 0, 0);


          bookedHours.forEach(bh => {
            const startTime = new Date(bh.startDate).getTime();
            const endTime = new Date(bh.endDate).getTime();
            if (time.getTime() == startTime && time.getTime() == endTime) {
              isAvailable = false;
            }
          });
        }

        if (isAvailable) {
          const minutesFormatted = minutes < 10 ? `0${minutes}` : minutes;
          availableAppointment.push(`${hour}:${minutesFormatted}`);
        }
      }
    }

    return availableAppointment;
  }

  ngOnInit(): void {
    this.loadBookings();
      
  }

  handleDateClick(arg: any): void {
    const clickedDate = new Date(arg.date);
    const bookedDates= this.getBookedDates(clickedDate);
    this.choosenBookingDate = clickedDate;
    // Open the modal with available hours
    this.openAvailableHoursModal(clickedDate, bookedDates);
  }

  dateFormating(date: Date): string {
    return new Date(date).toLocaleString('en-US', {
      year: 'numeric',   // e.g., "2024"
      month: 'short',    // e.g., "Aug"
      day: 'numeric',    // e.g., "31"
      hour: '2-digit',   // e.g., "14"
      minute: '2-digit', // e.g., "30"
      hour12: false      // Use 24-hour format (set to true for 12-hour format with AM/PM)
    });
  }

  getBookedDates(date: Date): any[] {
    const formatedDate: string = this.dateFormating(date);
    var bookedHours: any[] = [];
    this.reservations.forEach(r => {
      const createdAtFormated: string = this.dateFormating(r.createdAt!); // m/d/y/h:m
      const endingAtFormated: string = this.dateFormating(r.endingAt!);
      const createdAtWithoutHoursMins = createdAtFormated.split(",")[0] + createdAtFormated.split(",")[1] ;
      const formatedDateWithoutHoursMins = formatedDate.split(",")[0] + formatedDate.split(",")[1]
      if (createdAtWithoutHoursMins == formatedDateWithoutHoursMins) {
        const dateObject: StartEndDate = { startDate: new Date(createdAtFormated), endDate: new Date(endingAtFormated) };
          bookedHours.push(dateObject);
      }
    })
    console.log(bookedHours);
    return bookedHours;
    //return this.reservations
    //  .filter(reservation => new Date(reservation.createdAt).toDateString() === date.toDateString())
    //  .map(reservation => new Date(reservation.createdAt));
  }

  openAvailableHoursModal(date: Date, bookedHours: StartEndDate[]): void {
    const availableHours = this.computeAvailableHours(bookedHours);

    this.availableHoursModalRef = this.modalService.open(AvailableHoursModalComponent, {backdrop:false, size:'md'});
    this.availableHoursModalRef.componentInstance.date = date;
    this.availableHoursModalRef.componentInstance.availableHours = availableHours;
    this.availableHoursModalRef.componentInstance.OnClose.subscribe(() => {
      this.closeAvaiableHoursModal();
    });
    this.availableHoursModalRef.componentInstance.OnSubmit.subscribe((currentHours: string) => {
      //const [hours, mins] = currentHours.split(":");
      const [hours, mins] = currentHours.split(":");
      this.choosenBookingDate.setHours(parseInt(hours));
      this.choosenBookingDate.setMinutes(parseInt(mins));
      this.dateSelected.emit(this.choosenBookingDate);
      alert("Hours selected:" + currentHours);
      //this.confirmAvaiableHourseModal();
    })
  }

  closeAvaiableHoursModal() {
    this.availableHoursModalRef.close();
  }

  //confirmAvaiableHourseModal() {
  //  this.modalService.open(SubmitModalComponent, { backdrop: false });

  //  this.submitModalRef.componentInstance.OnClose.subscribe(() => {
  //    this.closeSubmitModal();
  //  })
  //  this.submitModalRef.componentInstance.OnSubmit.subscribe((selectedHour: string) => {
  //    this.confirmSubmitModal(this.choosenBookingTime);
  //  })
  //}

  

  closeSubmitModal() {
    this.submitModalRef.close();
  }

  //confirmSubmitModal(selectedHour: string) {
  //  const [hour, minutes] = selectedHour.split(":");
  //  this.choosenBookingDate.setMinutes(parseInt(minutes));
  //  this.choosenBookingDate.setHours(parseInt(hour));
  //}

  openSubmitModal() {
    this.submitModalRef = this.modalService.open(SubmitModalComponent, { backdrop: true });
    this.submitModalRef.componentInstance.OnClose.subscribe(() => {
      this.closeSubmitModal();
    })
  }

}

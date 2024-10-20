import { HttpClient } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { CalendarOptions } from '@fullcalendar/core'
import dayGridPlugin from '@fullcalendar/daygrid'
import interactionPlugin from '@fullcalendar/interaction'
import { Reservation } from '../../../models/reservation.model';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AvailableHoursModalComponent } from '../modal/available-hours-modal/available-hours-modal.component';
import { SubmitModalComponent } from 'src/app/modules/modal/submit-modal/submit-modal.component';

@Component({
  selector: 'app-booking-calendar',
  templateUrl: './booking-calendar.component.html',
  styleUrls: ['./booking-calendar.component.css']
})
export class BookingCalendarComponent implements OnInit {
  private readonly minHour: number = 11;
  private readonly maxHour: number = 15;
  private readonly gapMinutes: number = 20; // it will be changed to booking.service.length
  private reservations: Reservation[] = [];
  private availableHoursModalRef!: NgbModalRef;
  private modalService = inject(NgbModal);
  private submitModalRef!: NgbModalRef;
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
          title: booking.userId.toString(),
          start: new Date(booking.createdAt),
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

  computeAvailableHours(bookedHours: Date[]): string[] {
    const hours = [];

    for (let hour = this.minHour; hour < this.maxHour; hour++) {
      for (let minutes = 0; minutes < 60; minutes += this.gapMinutes) {
        const time = new Date();
        time.setHours(hour, minutes, 0, 0);

        if (!bookedHours.some(booked => booked.getHours() === hour && booked.getMinutes() === minutes)) {
          hours.push(time.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }));
        }
      }
    }
    return hours;
  }

  ngOnInit(): void {
    this.loadBookings();
      
  }

  handleDateClick(arg: any): void {
    const clickedDate = new Date(arg.date);
    const bookedHours = this.getBookedHours(clickedDate);

    // Open the modal with available hours
    this.openAvailableHoursModal(clickedDate, bookedHours);
    console.log(this.computeAvailableHours(bookedHours));
  }

  getBookedHours(date: Date): Date[] {
    return this.reservations
      .filter(reservation => new Date(reservation.createdAt).toDateString() === date.toDateString())
      .map(reservation => new Date(reservation.createdAt));
  }

  openAvailableHoursModal(date: Date, bookedHours: Date[]): void {
    const availableHours = this.computeAvailableHours(bookedHours);

    this.availableHoursModalRef = this.modalService.open(AvailableHoursModalComponent, {backdrop:false, size:'md'});
    this.availableHoursModalRef.componentInstance.date = date;
    this.availableHoursModalRef.componentInstance.availableHours = availableHours;
    this.availableHoursModalRef.componentInstance.OnClose.subscribe(() => {
      this.closeAvaiableHoursModal();
    });
    this.availableHoursModalRef.componentInstance.OnSubmit.subscribe(() => {
      this.confirmAvaiableHourseModal();
    })
  }

  closeAvaiableHoursModal() {
    this.availableHoursModalRef.close();
  }

  confirmAvaiableHourseModal() {
    this.submitModalRef = this.modalService.open(SubmitModalComponent, { backdrop: false });
    this.submitModalRef.componentInstance.OnClose.subscribe(() => {
      this.closeSubmitModal();
    })
    this.submitModalRef.componentInstance.OnSubmit.subscribe(() => {
      this.confirmSubmitModal();
    })
  }

  closeSubmitModal() {
    this.submitModalRef.close();
  }

  confirmSubmitModal() {
    alert("Submit modal submited");
  }

  openSubmitModal() {
    this.submitModalRef = this.modalService.open(SubmitModalComponent, { backdrop: true });
    this.submitModalRef.componentInstance.OnClose.subscribe(() => {
      this.closeSubmitModal();
    })
  }

}

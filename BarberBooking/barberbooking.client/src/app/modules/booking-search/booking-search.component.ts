import { Component, EventEmitter, Input, Output, TemplateRef, inject } from '@angular/core';
import { Reservation } from '../../../models/reservation.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap'

@Component({
  selector: 'app-booking-search',
  templateUrl: './booking-search.component.html',
  styleUrls: ['./booking-search.component.css']
})
export class BookingSearchComponent {

  @Output() DateRecived = new EventEmitter<Date>();
  allReservations: Reservation[] = [];
  private modalService = inject(NgbModal);
  private selectedDate?: Date;
  constructor() {

  }

  OnDateRecived() {
    this.DateRecived.emit(this.selectedDate);
  }

  OnDateSelected(date: Date) {
    this.selectedDate = date;
    this.OnDateRecived();
  }

  open(template:TemplateRef<any>){
    this.modalService.open(template, {size:'lg', backdrop: true });
  }

}

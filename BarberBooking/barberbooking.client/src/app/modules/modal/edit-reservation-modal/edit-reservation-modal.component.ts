import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ServiceType } from '../../../../models/service-type.model';
import { ToastrService } from 'ngx-toastr';
import { DateEmiterService } from '../../../services/date-emiter.service';
import { NewReservation } from '../../../../models/new-reservation.model';

@Component({
  selector: 'app-edit-reservation-modal',
  templateUrl: './edit-reservation-modal.component.html',
  styleUrls: ['./edit-reservation-modal.component.css']
})
export class EditReservationModalComponent implements OnInit {
  public serviceTypes?: ServiceType[];
  @Input() newReservationService!: ServiceType;
  @Input() newReservationDate: Date | null = null;
  @Output() OnClose = new EventEmitter();
  @Output() OnSubmit = new EventEmitter<NewReservation>();

  constructor(private httpClient: HttpClient, private notification:ToastrService, private dateEmitterService:DateEmiterService) {

  }

  close() {
    this.OnClose.emit();
  }
    
  submit() {

    this.newReservationDate = new Date(this.newReservationDate!);
    const dateOfEndingService: Date = new Date(this.newReservationDate!.getTime() + 30 * 60000);

    var newReservation: NewReservation = {
      serviceTypeId: this.newReservationService.id,
      dateOfReservation: this.newReservationDate,
      dateOfEndingService: dateOfEndingService
    }
    this.OnSubmit.emit(newReservation);
  }




  ngOnInit() {
    this.dateEmitterService.newDate$.subscribe((date: Date | null) => {
      this.newReservationDate = date;
    })
    this.httpClient.get<ServiceType[]>("http://localhost:5137/api/service-type").subscribe(
      (result) => {
        this.serviceTypes = result;
      },
      (error) => {
        this.notification.error(error.message)
      }
    )
    
  }
  
}

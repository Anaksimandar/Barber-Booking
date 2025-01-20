import { Component, OnInit } from '@angular/core';
import { ServiceType } from '../../../models/service-type.model';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { NewReservation } from '../../../models/new-reservation-model';
import { DateEmiterService } from '../../services/date-emiter.service';
import { RestService } from '../rest/rest-service';

@Component({
  selector: 'app-create-reservation',
  templateUrl: './create-reservation.component.html',
  styleUrls: ['./create-reservation.component.css']
})
export class CreateReservationComponent implements OnInit{
  
  public selectedType!: ServiceType | null;
  serviceTypes: ServiceType[] = [];
  public selectedDate!: Date | null;

  constructor(
    private httpClient: HttpClient,
    private notification: ToastrService,
    private restService:RestService,
    private dateEmiterService: DateEmiterService) {

  }

  restartForm() {
  }

  addReservation(): void {
    var serviceTypeId: number | null = this.selectedType!.id;
    var newReservation: NewReservation = { serviceTypeId: serviceTypeId, dateOfReservation: this.selectedDate };

    this.restService.post("reservation", newReservation).subscribe({
      next: (result) => {
        this.notification.success("Reservation has been created succesfuly");
        this.selectedDate = null;
        this.selectedType = null;
      },
      error: (error) => {
        console.log(error);
        this.notification.error(error.error)
      }
    })
    //this.formGroup.reset();
  }


  ngOnInit() {
    this.dateEmiterService.existingDate$.subscribe((date:Date | null) => {
      this.selectedDate = date;
    })

    this.restService.get("service-type").subscribe({
      next: (result) => {
        this.serviceTypes = result;
      },
      error: (error) => {
        this.notification.error(error.message)
      }
    })
  }



}

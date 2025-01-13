import { Component, OnInit } from '@angular/core';
import { ServiceType } from '../../../models/service-type.model';
import { HttpClient } from '@angular/common/http';
import { Reservation } from '../../../models/reservation.model';
import { ToastrService } from 'ngx-toastr';
import { NewReservation } from '../../../models/new-reservation-model';
import { DateEmiterService } from '../../services/date-emiter.service';

@Component({
  selector: 'app-create-reservation',
  templateUrl: './create-reservation.component.html',
  styleUrls: ['./create-reservation.component.css']
})
export class CreateReservationComponent implements OnInit{
  
  public selectedType!: ServiceType;
  serviceTypes: ServiceType[] = [];
  public selectedDate!: Date | null;

  constructor(private httpClient: HttpClient, private notification:ToastrService, private dateEmiterService:DateEmiterService) {

  }

  addReservation(): void {
    var serviceTypeId: number = this.selectedType.id;
    var newReservation: NewReservation = { serviceTypeId: serviceTypeId, dateOfReservation: this.selectedDate};
    this.httpClient.post("https://localhost:7030/api/reservation", newReservation).subscribe(
      (result: any) => {
        this.notification.success("Reservation has been created succesfuly");
        this.selectedDate = null;
      },
      error => {
        console.log(error);
        this.notification.error(error.error)
      }
    )
  }


  ngOnInit() {
    this.dateEmiterService.existingDate$.subscribe((date:Date | null) => {
      this.selectedDate = date;
    })
    this.httpClient.get<ServiceType[]>("https://localhost:7030/api/service-type").subscribe(
      (result) => {
        this.serviceTypes = result;
      },
      (error) => {
        this.notification.error(error.message)
      }
    )
  }



}

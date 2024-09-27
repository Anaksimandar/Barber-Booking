import { Component, OnInit } from '@angular/core';
import { ServiceType } from '../../../models/service-type.model';
import { HttpClient } from '@angular/common/http';
import { Reservation } from '../../../models/reservation.model';
import { ToastrService } from 'ngx-toastr';
import { NewReservationModel } from '../../../models/new-reservation-model';

@Component({
  selector: 'app-create-reservation',
  templateUrl: './create-reservation.component.html',
  styleUrls: ['./create-reservation.component.css']
})
export class CreateReservationComponent implements OnInit{
  
  public selectedType!: ServiceType;
  serviceTypes: ServiceType[] = [];
  public selectedDate!: Date;

  constructor(private httpClient: HttpClient, private notification:ToastrService) {

  }

  addReservation(): void {
    var serviceTypeId: number = this.selectedType.id;
    var newReservation: NewReservationModel = { serviceTypeId: serviceTypeId, dateOfReservation: this.selectedDate, userId: 1 };
    this.httpClient.post("https://localhost:7030/api/reservation", newReservation).subscribe(
      (result: any) => {
        this.notification.success("Reservation has been created succesfuly");
      },
      error => {
        this.notification.error(error.message)
      }
    )
  }

  onDateRecived(date: Date) {
    this.selectedDate = date;
  }

  ngOnInit() {
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

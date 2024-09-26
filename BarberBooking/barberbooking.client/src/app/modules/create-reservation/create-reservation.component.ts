import { Component, OnInit } from '@angular/core';
import { ServiceType } from '../../../models/service-type.model';
import { HttpClient } from '@angular/common/http';
import { Reservation } from '../../../models/reservation.model';

@Component({
  selector: 'app-create-reservation',
  templateUrl: './create-reservation.component.html',
  styleUrls: ['./create-reservation.component.css']
})
export class CreateReservationComponent implements OnInit{
  
  public selectedType!: ServiceType;
  serviceTypes: ServiceType[] = [];
  public selectedDate?: Date;

  constructor(private httpClient:HttpClient) {

  }

  addReservation(): void {
    var serviceTypeId: number = this.selectedType.id;
    alert(serviceTypeId);
    this.httpClient.post<any>("https://localhost:7030/api/reservation", serviceTypeId).subscribe(
        (result: any) => {
        alert(result);
      },
      error => alert(error)
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
        alert(error);
      }
    )
  }



}

import { Component, OnInit } from '@angular/core';
import { ServiceType } from '../../../models/service-type.model';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { NewReservation } from '../../../models/new-reservation-model';
import { DateEmiterService } from '../../services/date-emiter.service';

@Component({
  selector: 'app-create-reservation',
  templateUrl: './create-reservation.component.html',
  styleUrls: ['./create-reservation.component.css']
})
export class CreateReservationComponent implements OnInit{
  
  public selectedType!: ServiceType | null;
  serviceTypes: ServiceType[] = [];
  public selectedDate!: Date | null;

  constructor(private httpClient: HttpClient, private notification:ToastrService, private dateEmiterService:DateEmiterService) {

  }

  restartForm() {
  }

  addReservation(): void {
    var serviceTypeId: number | null = this.selectedType!.id;
    var newReservation: NewReservation = { serviceTypeId: serviceTypeId, dateOfReservation: this.selectedDate};
    this.httpClient.post("http://localhost:5137/api/reservation", newReservation).subscribe(
      (result: any) => {
        this.notification.success("Reservation has been created succesfuly");
        this.selectedDate = null;
        this.selectedType = null;
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

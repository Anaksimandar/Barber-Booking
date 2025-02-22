import { Component, OnInit } from '@angular/core';
import { ServiceType } from '../../../models/service-type.model';
import { ToastrService } from 'ngx-toastr';
import { NewReservation } from '../../../models/new-reservation.model';
import { DateEmiterService } from '../../services/date-emiter.service';
import { RestService } from '../rest/rest-service';
import { Router } from '@angular/router';
import { Observable, catchError, of, tap } from 'rxjs';

@Component({
  selector: 'app-create-reservation',
  templateUrl: './create-reservation.component.html',
  styleUrls: ['./create-reservation.component.css']
})
export class CreateReservationComponent implements OnInit {

  public selectedType!: ServiceType | null;
  serviceTypes$!: Observable<ServiceType[]>;
  public selectedDate!: Date | null;
  public creatingReservation: boolean = false;

  constructor(
    private notification: ToastrService,
    private restService: RestService,
    private router: Router,
    private dateEmiterService: DateEmiterService) {

  }

  restartForm() {
  }

  addReservation(): void {
    this.creatingReservation = true;
    var serviceTypeId: number | null = this.selectedType!.id;
    const dateOfReservation: Date = new Date(this.selectedDate!.toISOString());
    const dateOfEndingService = new Date(this.selectedDate!.getTime() + 30 * 60000);
    const dateOfEndingServiceFormated = new Date(dateOfEndingService.toISOString());
    var newReservation: NewReservation = {
      serviceTypeId: serviceTypeId,
      dateOfReservation: dateOfReservation,
      dateOfEndingService: dateOfEndingServiceFormated
    };

    this.restService.post("reservation", newReservation).subscribe({
      next: (result) => {
        this.notification.success("Reservation has been created succesfuly");
        this.notification.success(result.message);
        this.selectedDate = null;
        this.selectedType = null;
        this.creatingReservation = false;
      },
      error: (error) => {
        console.log(error);
        this.notification.error(error)
        this.creatingReservation = false;
      }
    })

  }


  ngOnInit() {
    this.dateEmiterService.existingDate$.subscribe((date: Date | null) => {
      this.selectedDate = date;
    })

    this.serviceTypes$ = this.restService.get("service-type").pipe(
      catchError(err => {
        this.notification.error(err);
        return of([]);
      })
    )
  }
}

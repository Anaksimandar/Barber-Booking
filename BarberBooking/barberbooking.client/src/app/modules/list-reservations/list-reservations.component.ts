import { Component, OnInit } from '@angular/core';
import { Reservation } from '../../../models/reservation.model';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-list-reservations',
  templateUrl: './list-reservations.component.html',
  styleUrls: ['./list-reservations.component.css']
})
export class ListReservationsComponent implements OnInit {
  public allReservations: Reservation[];

  constructor(private httpClient:HttpClient, private notification:ToastrService) {
    this.allReservations = [];
  }
  ngOnInit() {
    this.getAllReservations();
  }

  getAllReservations() {
    this.httpClient.get<Reservation[]>("https://localhost:7030/api/reservation").subscribe(
      result => {
        this.allReservations = result;
      },
      error => {
        this.notification.error(error.message);
      }
    )
  }

}

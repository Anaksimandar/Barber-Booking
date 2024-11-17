import { Component, Inject, OnInit } from '@angular/core';
import { Reservation } from '../../../models/reservation.model';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { EditReservationModalComponent } from '../modal/edit-reservation-modal/edit-reservation-modal.component';
import { DateEmiterService } from '../../services/date-emiter.service';
import { NewReservation } from '../../../models/new-reservation-model';
import { AccountService } from '../../services/account.service';
import { Observable, take } from 'rxjs';
import { AuthenticatedUser } from '../../../models/authenticated-user.model';
import { RoleType } from '../../../models/role-type.model';

@Component({
  selector: 'app-list-reservations',
  templateUrl: './list-reservations.component.html',
  styleUrls: ['./list-reservations.component.css']
})
export class ListReservationsComponent implements OnInit {
  public allReservations: Reservation[];
  private editModalRef!: NgbModalRef;
  private currenReservation?: Reservation;
  public currentUser!: AuthenticatedUser | null;
  public isAdmin!: boolean;

  constructor(
    private httpClient: HttpClient,
    private notification: ToastrService,
    private modalService: NgbModal,
    private dateEmitter: DateEmiterService,
    private accountService: AccountService
  ){
    this.allReservations = [];
    this.accountService.currentUser$.pipe(take(1)).subscribe(
      result => {
        this.currentUser = result;
        this.isAdmin = this.currentUser?.role.roleType == RoleType.Admin;
      }
    );
    
  }
  ngOnInit() {
    this.getAllReservations();
    
  }

  openEditReservationModal(reservation: Reservation) {
    this.currenReservation = reservation;
    this.dateEmitter.setNewDate(reservation.dateOfReservation);
    
    this.editModalRef = this.modalService.open(EditReservationModalComponent);
    this.editModalRef.componentInstance.newReservationDate = reservation.dateOfReservation;
    this.editModalRef.componentInstance.newReservationService = reservation.serviceType;
    this.editModalRef.componentInstance.OnClose.subscribe(() => {
      this.closeEditModal()
    })
    this.editModalRef?.componentInstance.OnSubmit.subscribe((newReservation:NewReservation) => {
      this.submitEditModalRef(newReservation)
    })
  }

  closeEditModal() {
    this.editModalRef?.close();
  }

  submitEditModalRef(newReservation:NewReservation) {
    this.editReservation(newReservation)
    this.closeEditModal();
  }

  editReservation(newReservation: NewReservation) {
    this.httpClient.put(`https://localhost:7030/api/reservation/${this.currenReservation?.id}`, newReservation).subscribe(
      result => {
        this.notification.success("Reservation has been updated successfully");
        this.getAllReservations();
      },
      error => {
        this.notification.error(error.error.message);
      }
    )
  }


  deleteReservation(reservationId: number) {
    this.httpClient.delete(`https://localhost:7030/api/reservation/${reservationId}`).subscribe(
      result => {
        this.notification.success("Reservation has been successfully deleted");
        this.getAllReservations();
      },
      error => {
        this.notification.error("Error with deleting reservation");
      }
    )
  }

  getAllReservations() {
    this.httpClient.get<Reservation[]>("https://localhost:7030/api/reservation").subscribe(
      result => {
        this.allReservations = result;
        console.log(result);
      },
      error => {
        this.notification.error(error.message);
      }
    )
  }

  //editReservation(reservaton:Reservation) {
    
  //}

}

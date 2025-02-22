import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Reservation } from '../../../models/reservation.model';
import { ToastrService } from 'ngx-toastr';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { EditReservationModalComponent } from '../modal/edit-reservation-modal/edit-reservation-modal.component';
import { DateEmiterService } from '../../services/date-emiter.service';
import { NewReservation } from '../../../models/new-reservation.model';
import { AccountService } from '../../services/account.service';
import { Observable, catchError, of, take, tap } from 'rxjs';
import { AuthenticatedUser } from '../../../models/authenticated-user.model';
import { RoleType } from '../../../models/role-type.model';
import { RestService } from '../rest/rest-service';

@Component({
  selector: 'app-list-reservations',
  templateUrl: './list-reservations.component.html',
  styleUrls: ['./list-reservations.component.css']
})
export class ListReservationsComponent implements OnInit, AfterViewInit{
  public reservations!: Reservation[];
  private editModalRef!: NgbModalRef;
  private currenReservation!: Reservation;
  public currentUser!: AuthenticatedUser | null;
  public isAdmin!: boolean;
  public isLoading: boolean = false;
  public initialData: Reservation[];
  displayedColumns: string[];

  constructor(
    private notification: ToastrService,
    private modalService: NgbModal,
    private dateEmitter: DateEmiterService,
    private restService:RestService,
    private accountService: AccountService
  ){
    this.accountService.currentUser$.pipe(take(1)).subscribe(
      result => {
        this.currentUser = result;
        this.isAdmin = this.currentUser?.role.roleType == RoleType.Admin;
      }
    );
    this.initialData = [];
    this.reservations = [];
    this.displayedColumns = ['user', 'serviceType', 'dateOfReservation', 'actions'];
  }

  ngAfterViewInit() {
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
    this.editReservation(newReservation
    )
    this.closeEditModal();
  }

  editReservation(newReservation: NewReservation) {
    this.isLoading = true;
    this.restService.update("reservation", this.currenReservation?.id, newReservation).subscribe({
      next: (result) => {
        this.notification.success("Reservation has been updated successfully");
        this.dateEmitter.setExistingDate(null);
        this.getAllReservations()
        this.isLoading = false;
      },
      error: (error) => {
        this.notification.error(error);
        this.isLoading = false;
      }
    })
  }


  deleteReservation(reservationId: number) {
    this.isLoading = true;
    this.restService.delete("reservation", reservationId).subscribe({
      next: (result) => {
        this.notification.success("Reservation has been successfully deleted");
        this.getAllReservations();
        this.isLoading = false;
      },
      error: (error) => {
        this.notification.error("Error with deleting reservation");
        this.isLoading = false;
      }
    })
  }

  getAllReservations() {
    this.isLoading = true;
    this.restService.get("reservation").subscribe(
      res => {
        this.reservations = res;
        this.initialData = this.reservations.slice(0, 5);
        this.isLoading = false;
      },
      err => {
        this.notification.error(err.message);
        this.isLoading = false;
      }
    )
  }

  //editReservation(reservaton:Reservation) {
    
  //}

}

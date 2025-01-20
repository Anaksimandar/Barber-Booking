import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NotificationService } from '../../services/notification.service';
import { RestService } from '../rest/rest-service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  formGroup!: FormGroup;

  constructor(
    private http: HttpClient,
    private restService:RestService,
    private notification: NotificationService) {

  }

  ngOnInit(): void {
    this.initializeForm();
  }

  sendConfirmationLink() {
    var mail: string = this.formGroup.get("mail")?.value;
    this.restService.post("forgot-password", mail).subscribe({
      next: (result) => {
        this.notification.showSuccess("Confirmation link has been send to " + mail)
      },
      error: (error) => {
        this.notification.showError(error.error);
      }
    })
    this.formGroup.reset();
  }
  
  initializeForm() {
    this.formGroup = new FormGroup({
      mail: new FormControl("", [Validators.required, Validators.email]),
    })
  }
}

import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  formGroup!: FormGroup;

  constructor(private http:HttpClient, private notification:NotificationService) {

  }

  ngOnInit(): void {
    this.initializeForm();
  }

  sendConfirmationLink() {
    var httpOptions:object = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    }

    var mail: string = this.formGroup.get("mail")?.value;
    this.http.post("http://localhost:5137/api/forgot-password", JSON.stringify(mail), httpOptions).subscribe(
      result => {
        this.notification.showSuccess("Confirmation link has been send to " + mail)
      },
      error => {
        this.notification.showError(error.error);
      }
    )
    this.formGroup.reset();
  }
  
  initializeForm() {
    this.formGroup = new FormGroup({
      mail: new FormControl("", [Validators.required, Validators.email]),
    })
  }
}

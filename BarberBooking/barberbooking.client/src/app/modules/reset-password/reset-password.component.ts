import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { RestartPasswordModel } from '../../../models/restart-password.model';
import { NotificationService } from '../../services/notification.service';
import { RestService } from '../rest/rest-service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  formGroup!: FormGroup;
  private userId!:number;
  private passwordToken!:string | null;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private restService:RestService,
    private notification: NotificationService) {
    this.route.queryParamMap.subscribe(params => {
      const userIdParam = params.get("userId");
      const passwordTokenParam = params.get("token");
      this.userId = userIdParam ? parseInt(userIdParam) : 0;
      this.passwordToken = passwordTokenParam ? passwordTokenParam : "";
    })
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  changePassword() {
    const restartPasswordModel: RestartPasswordModel = {
      userId: this.userId,
      passwordToken: this.passwordToken,
      newPassword: this.formGroup.get("newPassword")?.value,
      confirmNewPassword: this.formGroup.get("confirmNewPassword")?.value
    };

    this.restService.post("reset-password", restartPasswordModel).subscribe({
      next: (result) => {
        this.notification.showSuccess("Password has been changed succesfully");
      },
      error: (error) => {
        this.notification.showError(error.error);
      }
    })
    this.formGroup.reset();
  }

  initializeForm() {
    this.formGroup = new FormGroup({
      newPassword: new FormControl("", [Validators.required, Validators.minLength(6)]),
      confirmNewPassword: new FormControl("", [Validators.required, Validators.minLength(6)])
    })
  }
}

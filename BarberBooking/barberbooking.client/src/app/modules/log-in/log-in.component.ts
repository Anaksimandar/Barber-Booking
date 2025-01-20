import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { LoginUser } from '../../../models/login.model';
import { HttpClient } from '@angular/common/http';
import { AccountService } from '../../services/account.service';
import { LoginResponse } from '../../../models/login-response.model';
import { Router } from '@angular/router';
import { RestService } from '../rest/rest-service';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.css']
})
export class LogInComponent implements OnInit {
  loginForm!: FormGroup;

  constructor(
    private notification: ToastrService,
    private restService:RestService,
    private http: HttpClient,
    private accountService: AccountService,
    private router: Router
  ) { }

  ngOnInit() {
    this.initializeForm();
  }

  initializeForm() {
    this.loginForm = new FormGroup({
      email: new FormControl("", [Validators.required, Validators.email]),
      password: new FormControl("", [Validators.required, Validators.minLength(6)]),
    })
  }

  login(user: LoginUser) {
    this.restService.post("login", user).subscribe({
      next: (result) => {
        this.notification.success("You logged in successfully " + result.user.name);
        this.accountService.loginUser(result);
        this.router.navigateByUrl("/");
      },
      error: (error) => {
        this.notification.error(error);
      }
    })
    this.loginForm.reset();
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.login(this.loginForm.value);
      this.loginForm.reset();
    }

  }
}

import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NewUser } from '../../../models/new-user.model';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent {
  signUpForm!: FormGroup;

  constructor(private notification: ToastrService, private http:HttpClient) { }

  ngOnInit() {
    this.initializeForm();
  }

  initializeForm() {
    this.signUpForm = new FormGroup({
      name: new FormControl("", [Validators.required, Validators.minLength(3)]),
      surname: new FormControl("", [Validators.required, Validators.minLength(3)]),
      email: new FormControl("", [Validators.required, Validators.email]),
      password: new FormControl("", [Validators.required, Validators.minLength(6)]),
      confirmPassword: new FormControl("", [Validators.required, Validators.minLength(6)]),
    })
  }

  addUser(newUser: NewUser) {
    this.http.post("https://localhost:7030/api/sign-in", newUser).subscribe(
      result => {
        this.notification.success("You sign up successfully" + result);
      },
      error => {
        this.notification.error(error);
      }
    )
  }

  onSubmit() {
    if (this.signUpForm.valid) {
      this.addUser(this.signUpForm.value);
      this.signUpForm.reset();
    }

  }

}

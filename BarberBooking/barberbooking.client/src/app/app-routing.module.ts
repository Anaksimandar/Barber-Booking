import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './modules/home/home.component';
import { ListReservationsComponent } from './modules/list-reservations/list-reservations.component';
import { SignUpComponent } from './modules/sign-up/sign-up.component';
import { LogInComponent } from './modules/log-in/log-in.component';
import { AuthGuard } from './guards/auth.guard';
import { ForgotPasswordComponent } from './modules/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './modules/reset-password/reset-password.component';
import { ChangePasswordComponent } from './modules/change-password/change-password.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full'},
  { path: 'home', component: HomeComponent, canActivate:[AuthGuard] },
  { path: 'list-reservations', component: ListReservationsComponent,canActivate:[AuthGuard] },
  { path: 'sign-up', component: SignUpComponent },
  { path: 'login', component: LogInComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  { path: 'change-password', component: ChangePasswordComponent }

]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

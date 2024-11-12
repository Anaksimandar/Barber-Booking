import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './modules/home/home.component';
import { ListReservationsComponent } from './modules/list-reservations/list-reservations.component';
import { SignUpComponent } from './modules/sign-up/sign-up.component';
import { LogInComponent } from './modules/log-in/log-in.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'list-reservations', component: ListReservationsComponent,canActivate:[AuthGuard] },
  { path: 'sign-up', component: SignUpComponent },
  { path: 'login', component: LogInComponent}

]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

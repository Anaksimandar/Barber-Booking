import { Injectable } from '@angular/core';
import { LoginResponse } from '../../models/login-response.model';
import { BehaviorSubject, Observable } from 'rxjs';
import { AuthenticatedUser } from '../../models/authenticated-user.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private currentUserSubject: BehaviorSubject<AuthenticatedUser | null>;
  public currentUser$: Observable<AuthenticatedUser | null>;
  constructor() {
    this.currentUserSubject = new BehaviorSubject<AuthenticatedUser | null>(this.getCurrentUserFromStorage());
    this.currentUser$ = this.currentUserSubject.asObservable()
  }

  loginUser(response:LoginResponse) {
    localStorage.setItem("token", response.token);
    localStorage.setItem("user", JSON.stringify(response.user))
  }

  getToken():string | null {
    return localStorage.getItem("token");
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    if (!token && this.isTokenExpired(token)) {
      return false;
    }
    return true;
  }

  isTokenExpired(token: string | null) {
    if (!token) {
      return true;
    }
    const expiration = (JSON.parse(atob(token.split(".")[1]))).exp;
    return expiration < Math.floor(Date.now() / 1000);
  }

  getCurrentUserFromStorage(): AuthenticatedUser | null {
    if (!this.isAuthenticated) {
      return null;
    }
    const user = localStorage.getItem("user");
    return user ? JSON.parse(user) : null;
  }

  logout() {
    localStorage.removeItem("user");
    localStorage.removeItem("token")

    if (this.currentUserSubject) {
      this.currentUserSubject.next(null)
    }
  }
}

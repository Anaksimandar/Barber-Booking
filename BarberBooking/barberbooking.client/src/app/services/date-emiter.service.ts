import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DateEmiterService {
  private existingDate = new BehaviorSubject<Date | null>(null);
  private newDate = new BehaviorSubject<Date | null>(null);
  existingDate$ = this.existingDate.asObservable();
  newDate$ = this.newDate.asObservable();

  setExistingDate(date: Date) {
    this.existingDate.next(date);
  }

  setNewDate(date: Date) {
    this.newDate.next(date);
  }

  constructor() { }
}

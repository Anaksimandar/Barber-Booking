import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DateEmiterService {
  private selectedDate = new BehaviorSubject<Date | null>(null);
  selectedDate$ = this.selectedDate.asObservable();

  setDate(date: Date) {
    this.selectedDate.next(date);
  }

  constructor() { }
}

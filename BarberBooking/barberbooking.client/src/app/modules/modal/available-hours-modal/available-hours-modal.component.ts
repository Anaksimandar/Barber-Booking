import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import Swiper from 'swiper'; 

@Component({
  selector: 'app-available-hours-modal',
  templateUrl: './available-hours-modal.component.html',
  styleUrls: ['./available-hours-modal.component.css']
})
export class AvailableHoursModalComponent implements OnInit {
  @Input() date!: Date;
  @Input() availableHours!: string[];
  @Output() OnClose = new EventEmitter();
  @Output() OnSubmit = new EventEmitter<string>();

  currentHours!: string;
  constructor(public activeModal:NgbActiveModal) {

  }

  setCurrentHours(selectedHours:string) {
    this.currentHours = selectedHours;
  }

  close() {
    this.OnClose.emit();
  }

  submit() {
    this.OnSubmit.emit(this.currentHours);
    close();
  }


  ngOnInit(): void {
    const swiper = new Swiper('.swiper-container', {
      direction: 'vertical', // or 'horizontal'
      slidesPerView: 'auto',
      spaceBetween: 0,
      scrollbar: false,
      centeredSlides: true,
      slideToClickedSlide: true
    });
  }
  

  


}

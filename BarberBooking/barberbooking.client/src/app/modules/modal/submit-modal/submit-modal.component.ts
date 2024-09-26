import { AfterViewInit, Component, EventEmitter, Input, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-submit-modal',
  templateUrl: './submit-modal.component.html',
  styleUrls: ['./submit-modal.component.css']
})
export class SubmitModalComponent {
  @Output() OnClose = new EventEmitter();
  @Output() OnSubmit = new EventEmitter<string>();

  constructor(
    public acriveModal: NgbActiveModal,
  ) { }

  close() {
    this.OnClose.emit();
  }

  submit() {
    this.OnSubmit.emit();
  }
}

import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-submit-modal',
  templateUrl: './submit-modal.component.html',
  styleUrls: ['./submit-modal.component.css']
})
export class SubmitModalComponent {
  @Output() OnClose = new EventEmitter();
  @Output() OnSubmit = new EventEmitter();

  close() {
    this.OnClose.emit();
  }

  submit() {
    this.OnSubmit.emit();
  }


}

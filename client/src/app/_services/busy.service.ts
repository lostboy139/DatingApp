import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyRequestCount = 0;
  spinners = ['square-jelly-box', 'pacman', 'cog', 'ball-running-dots','ball-pulse-rise','ball-atom','ball-clip-rotate-multiple','ball-fussion'];

  constructor(private spinnerService: NgxSpinnerService) { }

  busy() {
    this.busyRequestCount++;
    this.spinnerService.show(undefined, {
      type: this.chooseSpinner(),
      bdColor: 'rgba(255,255,255,0)',
      color: '#333333'
    });
  }

  idle() {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.spinnerService.hide();
    }
  }

  chooseSpinner() {
    var spinner = this.spinners[Math.floor(Math.random() * 100 % this.spinners.length)];
    console.log(spinner);
    return spinner;
  }
}

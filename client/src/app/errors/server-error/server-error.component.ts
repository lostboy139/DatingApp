import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css']
})
export class ServerErrorComponent implements OnInit {
  error: any;
  state: any;

  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    //alert(navigation.extras.state);
    this.state = navigation?.extras?.state;
    this.error = this.state?.error;
    console.log(this.state)
  }

  ngOnInit(): void {
    //alert(history. state.error.details)
  }

}

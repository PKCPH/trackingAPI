import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-blank',
  templateUrl: './blank.component.html',
  styleUrls: ['./blank.component.css']
})
export class BlankComponent {

  customers: any;

  constructor(private http: HttpClient) {
    // this.http.get("https://localhost:5001/api/customers")
    // .subscribe({
    //   next: (result: any) => this.customers = result,
    //   error: (err: HttpErrorResponse) => console.log(err)
    // })
  }

}

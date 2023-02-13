import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.css']
})
export class CustomersComponent {

  customers: any;

  constructor(private http: HttpClient) {
    this.http.get("https://localhost:5001/api/customers")
    .subscribe({
      next: (result: any) => this.customers = result,
      error: (err: HttpErrorResponse) => console.log(err)
    })
  }

}

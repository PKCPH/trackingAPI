import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { LoginModel } from 'src/app/models/login.model';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private credentials = new BehaviorSubject<LoginModel>({userName: '', password: '', role: '', id: ''});
  currentCredentials = this.credentials.asObservable();

  constructor(private router: Router) { }

  updateCredentials(credentials: LoginModel) {
    this.credentials.next(credentials);
  }

}
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { LoginModel } from 'src/app/models/login.model';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

//Loginservice was created to maintain user credentials and update it whenever needed - mostly for saving it to localstorage and fetching it etc. - and for dynamically showing navbar items

  private credentials = new BehaviorSubject<LoginModel>({userName: '', password: '', role: '', id: '', balance: 0, email: ''});
  currentCredentials = this.credentials.asObservable();

  constructor() { }

  updateCredentials(credentials: LoginModel) {
    this.credentials.next(credentials);
  }

}
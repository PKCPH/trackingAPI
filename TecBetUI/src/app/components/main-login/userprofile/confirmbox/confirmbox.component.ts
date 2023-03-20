import { Component, Type } from '@angular/core';
import { Router } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { LoginModel } from 'src/app/models/login.model';
import { AuthguardService } from 'src/app/services/authguard.service';

@Component({
  selector: 'app-confirmbox',
  templateUrl: './confirmbox.component.html',
  styleUrls: ['./confirmbox.component.css']
})
export class ConfirmboxComponent {

  credentials: LoginModel = {userName:'', password:'', role: '', id: '00000000-0000-0000-0000-000000000000', balance: 0, email: ''};

	constructor(public modal: NgbActiveModal, private authService: AuthguardService, private router: Router) { 

    this.getCredentials();

  }

  getCredentials() {
    let storedCredentials;

    let storedCredentialsString = localStorage.getItem("credentials");
    if (storedCredentialsString)
    {
    storedCredentials = JSON.parse(storedCredentialsString);

    this.credentials.userName = storedCredentials.username;
    }
  }

  deleteUser(username: string) {
    this.authService.deleteUser(username)
    .subscribe({
      next: (response) => {
        localStorage.removeItem("jwt");
        localStorage.removeItem("credentials");
        this.router.navigate(['/'])
        this.modal.close('Ok click')
      }
    })
  }
}

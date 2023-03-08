import { Component, Type } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LoginModel } from 'src/app/models/login.model';
import { AuthguardService } from 'src/app/services/authguard.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmboxComponent } from './confirmbox/confirmbox.component';
import { ChangepasswordComponent } from './changepassword/changepassword.component';


@Component({
  selector: 'app-userprofile',
  templateUrl: './userprofile.component.html',
  styleUrls: ['./userprofile.component.css'],
})
export class UserprofileComponent {

  credentials: LoginModel = {userName:'', password:'', role: '', id: '00000000-0000-0000-0000-000000000000', balance: 0, email: ''};

  constructor(private authService: AuthguardService, private route: ActivatedRoute, private modalService: NgbModal) {

    this.route.paramMap.subscribe({
      next: (params) => {
        const userName = params.get('username');
        if (userName) {
          //Call API
this.authService.getUser(userName)
.subscribe({
  next: (response) => {
this.credentials.id = response.id
this.credentials.balance = response.balance,
this.credentials.email = response.email,
this.credentials.userName = response.userName,
this.credentials.role = response.role

// console.log(this.credentials);
  }
});
        }
      }
    })
  }

  openConfirm() {
		this.modalService.open(ConfirmboxComponent, {centered: true, size: 'lg', windowClass: 'modal-rounded'});
	}

  openPassword() {
		const ref = this.modalService.open(ChangepasswordComponent, { centered: true, windowClass: 'modal-rounded'});
    ref.componentInstance.selectedUser = this.credentials;
	}
}



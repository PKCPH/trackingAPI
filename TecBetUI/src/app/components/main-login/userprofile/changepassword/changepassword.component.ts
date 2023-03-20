import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Type } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthenticatedResponse } from 'src/app/models/AuthenticatedResponse';
import { LoginModel } from 'src/app/models/login.model';
import { AuthguardService } from 'src/app/services/authguard.service';

@Component({
  selector: 'app-changepassword',
  templateUrl: './changepassword.component.html',
  styleUrls: ['./changepassword.component.css']
})

export class ChangepasswordComponent {

  selectedUser: LoginModel | any;
  loginForm: FormGroup | any;
  submitted = false;
  errorMessage: string = "";
  timer: any;

  //modal variable is public because its used in the .html template part of the component.

  constructor(private authService: AuthguardService, private route: ActivatedRoute, public modal: NgbActiveModal, private formBuilder:
    FormBuilder, private router: Router, private http: HttpClient, private modalService: NgbModal) {

    this.buildValidator();

  }

  buildValidator() {
    this.loginForm = this.formBuilder.group({
      password: ['', [Validators.minLength(6), Validators.required]],
      passwordCheck: ['', [Validators.required]]
    });
  }

  //Updatepassword function - First it performs a bunch of validation checks, and also wrapped the auth function around a if statement that checks if the repeated password is correct.
  //It'll then "relog" you into the system, refreshing your existing token, and then it has multiple timers to open a modal that just states that you have changed your password succesfully

  updatePassword() {
    this.submitted = true;
    if (this.loginForm.valid) {
      if (this.selectedUser) {
        this.selectedUser = {
          ...this.selectedUser,
          password: this.loginForm.get('password')?.value,
          passwordCheck: this.loginForm.get('passwordCheck')?.value
        }
      }
      if (this.selectedUser.passwordCheck === this.selectedUser.password) {
        this.authService.updateUser(this.selectedUser.id, this.selectedUser)
          .subscribe({
            next: (response) => {
              this.router.navigate(['/dashboard']);
              this.http.post<AuthenticatedResponse>("https://localhost:5001/api/auth/login", this.selectedUser, {
                headers: new HttpHeaders({ "Content-Type": "application/json" })
              })
                .subscribe({
                  next: (response: AuthenticatedResponse) => {
                    const token = response.token;
                    const refreshToken = response.refreshToken;
                    localStorage.setItem("jwt", token);
                    localStorage.setItem("refreshToken", refreshToken);
                  }
                })
              this.modal.close();
              this.timer = setTimeout(() => {
                this.modalService.open(MODALS['confirm'], { centered: true, size: 'sm', windowClass: 'modal-confirmpass', keyboard: true });
              }, 1000);
              this.timer = setTimeout(() => {
                this.modalService.dismissAll();
              }, 3000);
            }
          });
      } else {
        this.errorMessage = 'Passwords do not match!';
      }
    } else {
      for (const key in this.loginForm.controls) {
        if (this.loginForm.controls.hasOwnProperty(key)) {
          const control = this.loginForm.get(key);
          if (control && control.invalid) {
            console.log(key, control.errors);
          }
        }
      }
    }
  }
}

//Custom mini component made to just display a short message

@Component({
  selector: 'confirm',
  template: `
  <div class="d-flex" style="justify-content: flex-end;">
</div>
		<div class="modal-body" style="text-align:center">
			<p class="">
				Your password has been changed.
			</p>
		</div>
	`,
})

export class NgbdModalConfirm {
  constructor(public modal: NgbActiveModal) { }
}

const MODALS: { [name: string]: Type<any> } = {
  confirm: NgbdModalConfirm
};







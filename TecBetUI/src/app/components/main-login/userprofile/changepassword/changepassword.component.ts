import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Input, Type } from '@angular/core';
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
  loginForm: FormGroup;
  submitted = false;
  errorMessage: string = "";
  timer: any;

  //modal variable is public because its used in the .html template part of the component.

  constructor(private authService: AuthguardService, private route: ActivatedRoute, public modal: NgbActiveModal, private formBuilder: 
    FormBuilder, private router: Router, private http: HttpClient, private modalService: NgbModal) { 

    this.loginForm = this.formBuilder.group({
      password: ['', [Validators.minLength(6), Validators.required]],
      passwordCheck: ['', [Validators.required]]
    });
  }

  updatePassword() {
    this.submitted = true;
    if(this.loginForm.valid)
    {
      if (this.selectedUser)
      {
        this.selectedUser = {
          ...this.selectedUser,
          password: this.loginForm.get('password')?.value,
          passwordCheck: this.loginForm.get('passwordCheck')?.value
        }   
      }

if(this.selectedUser.passwordCheck === this.selectedUser.password) {
    this.authService.updateUser(this.selectedUser.id, this.selectedUser)
    .subscribe({
      next: (response) => {
        this.router.navigate(['/dashboard']);
        this.http.post<AuthenticatedResponse>("https://localhost:5001/api/auth/login", this.selectedUser, {
          headers: new HttpHeaders({ "Content-Type": "application/json"})
        })
        .subscribe({
          next: (response: AuthenticatedResponse) => {
            const token = response.token;
            const refreshToken = response.refreshToken;
            localStorage.setItem("jwt", token); 
            localStorage.setItem("refreshToken", refreshToken);
      }})
        this.modal.close();
        this.timer = setTimeout(() => {
          this.modalService.open(MODALS['confirm'], {centered: true, size: 'sm'});
        }, 1000); 
        this.timer = setTimeout(() => {
          this.modalService.dismissAll();
        }, 3000); 
      }
    });
  } else {
    this.errorMessage = 'Passwords do not match!';
  }
  } else
  {
    for (const key in this.loginForm.controls) {
      if (this.loginForm.controls.hasOwnProperty(key)) {
        const control = this.loginForm.get(key);
        if (control && control.invalid) {
          console.log(key, control.errors);
  }
}
}}}

}

@Component({
	selector: 'confirm',
	template: `
  <div style="background-color: rgba(45,0,45,1)">
  <div class="d-flex" style="justify-content: flex-end;">
  <button
      type="button"
      ngbAutofocus
      class="btn-close my-2 mx-2"
      aria-label="Close button"
      aria-describedby="modal-title"
      (click)="modal.dismiss('Cross click')"
  ></button>
</div>
		<div class="modal-body" style="text-align:center">
			<p class="pb-3">
				Your password has been changed.
			</p>
		</div>
    </div>
	`,
})

export class NgbdModalConfirm {
	constructor(public modal: NgbActiveModal) {}
}

const MODALS: { [name: string]: Type<any> } = {
	confirm: NgbdModalConfirm
};







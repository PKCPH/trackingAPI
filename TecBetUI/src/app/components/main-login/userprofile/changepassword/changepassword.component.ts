import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
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

  constructor(private authService: AuthguardService, private route: ActivatedRoute, public modal: NgbActiveModal, private formBuilder: 
    FormBuilder, private router: Router) { 

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
        this.modal.close();
      }
    });
  } else {
    this.errorMessage = 'Passwords dont match!!';
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



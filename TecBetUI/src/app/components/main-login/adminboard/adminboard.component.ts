import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, Renderer2, OnDestroy, Type } from '@angular/core';
import { Router } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { interval, Subscription, switchMap } from 'rxjs';
import { LoginModel } from 'src/app/models/login.model';
import { AuthguardService } from 'src/app/services/authguard.service';

@Component({
  selector: 'app-adminboard',
  templateUrl: './adminboard.component.html',
  styleUrls: ['./adminboard.component.css']
})
export class AdminboardComponent implements OnDestroy {

  users: LoginModel[] = [];
  errorMessage: string = "";
  updateSubscription: Subscription;
  adminUsername: string = "";

  //ngOnDestroy is when you route out of a component it triggers, and inside it I unsubscribe to everything, so it doesnt keep running while on another component.

  ngOnDestroy() {
    this.updateSubscription.unsubscribe();
  }

  constructor(private authService: AuthguardService, private router: Router,
    private el: ElementRef, private renderer: Renderer2, private modalService: NgbModal) {

    //Basically this is the checker, to prevent users from trying to navigate to this url without being an admin it checks the localstorage for role credentials
    //If you dont have admin you'll be redirected to the homepage

    this.getCredentials();

    this.fetch();

    this.updateSubscription = interval(2500).subscribe(() => {
      this.fetch();
    });

  }

  fetch() {
    this.authService.getUsers().subscribe({
      next: (users) => {
        this.users = users.map(users => {
          return {
            ...users,
            password: '',
            id: users.id.substring(0, 8),
          }
        });
        // console.log(this.users)
        if (users) {
          this.Hideloader();
        }
        this.authService.errorMessage.subscribe(error => {
          this.errorMessage = error;
        });
      }
    });
  }

  getCredentials() {
    let storedCredentials;

    let storedCredentialsString = localStorage.getItem("credentials");
    if (storedCredentialsString) {
      storedCredentials = JSON.parse(storedCredentialsString);

      let role = storedCredentials.role;
      this.adminUsername = storedCredentials.username;

      if (role === 'Admin') {
        this.router.navigate(['/adminboard']);
      } else {
        this.router.navigate(['/']);
      }
    }
  }

  deleteUser(username: string) {
    this.authService.deleteUser(username)
      .subscribe({
        next: (response) => {
          this.authService.getUsers()
            .subscribe({
              next: (users) => {
                this.users = users.map(users => {
                  return {
                    ...users,
                    password: '',
                    id: users.id.substring(0, 8),
                  }
                });
              },
              error: (response) => {
                console.log(response);
              }
            });
        }
      });
  }

  openReset() {
    this.modalService.open(MODALS['confirm'], { centered: true, size: 'sm', windowClass: 'modal-confirmpass', keyboard: true });
  }

  //Hides loader, and shows relevant divs that should only appear after loading have finished.

  Hideloader() {
    // Setting display of spinner element to none
    this.renderer.setStyle(this.el.nativeElement.querySelector('#loading'), 'display', 'none');
    this.renderer.setStyle(this.el.nativeElement.querySelector('#admincontainer'), 'display', 'block');
    this.renderer.setStyle(this.el.nativeElement.querySelector('#adminpanel'), 'display', 'block');
  }
}

//Custom mini component made to just display a short message

@Component({
  selector: 'confirm',
  template: `
  <div class="modal-header">
  <h4 class="modal-title" id="modal-title">Reset</h4>
  <button
      type="button"
      class="btn-close"
      aria-label="Close button"
      aria-describedby="modal-title"
      (click)="modal.dismiss('Cross click')"
  ></button>
</div>
<div class="modal-body rounded-0" style="text-align: center;">
  <p>
      Are you sure you want to delete all matches?
</p>
</div>
<div class="modal-footer d-flex justify-content-center">
  <button type="button" class="btn btn-primary" (click)="modal.dismiss('cancel click')">Cancel</button>
  <button type="button" class="btn btn-danger" (click)="reset()">Confirm</button>
</div>
	`,
})

export class NgbdModalConfirm {
  constructor(public modal: NgbActiveModal, private http: HttpClient) { }

  reset() {
    this.http.get("https://localhost:5001/api/auth/reset").subscribe(() => {
      console.log("Request successful");
    }, (error) => {
      console.log("Error:", error);
    });
    this.modal.close();
  }
}

const MODALS: { [name: string]: Type<any> } = {
  confirm: NgbdModalConfirm
};

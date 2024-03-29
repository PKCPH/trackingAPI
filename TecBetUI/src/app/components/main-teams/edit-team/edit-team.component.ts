import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Team } from 'src/app/models/teams.model';
import { TeamsService } from 'src/app/services/teams.service';

@Component({
  selector: 'app-edit-team',
  templateUrl: './edit-team.component.html',
  styleUrls: ['./edit-team.component.css']
})
export class EditTeamComponent {

  teamForm: FormGroup | any;
  submitted = false;

  teamDetails: Team | any;

  constructor(private route: ActivatedRoute, private teamsService: TeamsService, private router: Router, private formBuilder: FormBuilder) {

    this.buildValidator();

    this.fetch();
   
  }

  fetch() {
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');
        if (id) {
          //Call API
          this.teamsService.getTeam(id)
            .subscribe({
              next: (response) => {
                this.teamDetails = response;
              }
            });
        }
      }
    })
  }

  //Here we use formbuilder to create validation, secures user inputs being wrong and gives a guideline to what the user needs to do to correct the value.

  buildValidator() {
    this.teamForm = this.formBuilder.group({
      name: ['', [Validators.required]]
      // Validators.pattern("^[a-zA-Z]*$")]
    });
  }

  updateTeam() {
    this.submitted = true;
    if (this.teamForm.valid) {
      if (this.teamDetails) {
        this.teamDetails = {
          ...this.teamDetails,
          name: this.teamForm.get('name')?.value
        }
      }
      this.teamsService.updateTeam(this.teamDetails.id, this.teamDetails)
        .subscribe({
          next: (response) => {
            this.router.navigate(['teams']);
          }
        });
    } else {
      for (const key in this.teamForm.controls) {
        if (this.teamForm.controls.hasOwnProperty(key)) {
          const control = this.teamForm.get(key);
          if (control && control.invalid) {
            console.log(key, control.errors);
          }
        }
      }
    }
  }
}
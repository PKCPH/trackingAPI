import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Team } from 'src/app/models/teams.model';
import { TeamsService } from 'src/app/services/teams.service';

@Component({
  selector: 'app-add-team',
  templateUrl: './add-team.component.html',
  styleUrls: ['./add-team.component.css']
})
export class AddTeamComponent {

  addTeamRequest: Team = {
    id: '',
    name: '',
    isAvailable: true,
    matches: [],
    availability: '',
    players: []
    availability: '',
    score: 0,
    result: 0
  };
  
  teamForm: FormGroup;
  submitted = false;
  
  constructor(private teamsService: TeamsService, private router: Router, private formBuilder: FormBuilder) {
    this.teamForm = this.formBuilder.group({
      name: ['', [Validators.required]]
      // Validators.pattern("^[a-zA-Z]*$")]
    });
  }
  
  addTeam() {
    this.submitted = true;
    if (this.teamForm.valid)
    {
      if (this.addTeamRequest)
      {
  this.addTeamRequest = {
  ...this.addTeamRequest,
  name: this.teamForm.get('name')?.value
  };
  }
  
    this.teamsService.addTeam(this.addTeamRequest)
    .subscribe({
      next: (members) => {
        this.router.navigate(['teams']);
      }
    });
  } else
  {
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

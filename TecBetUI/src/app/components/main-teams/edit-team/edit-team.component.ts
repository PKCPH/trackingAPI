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

  teamForm: FormGroup;
  submitted = false;

  teamDetails: Team = {
    id: '',
    name: '',
    IsAvailable: true,
    matches: [],
    availability: ''
  };

  constructor(private route: ActivatedRoute, private teamsService: TeamsService, private router: Router, private formBuilder: FormBuilder) {
   
    this.teamForm = this.formBuilder.group({
      name: ['', [Validators.required]]
      // Validators.pattern("^[a-zA-Z]*$")]
    });
    
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

  updateTeam() {
    this.teamsService.updateTeam(this.teamDetails.id, this.teamDetails)
    .subscribe({
      next: (response) => {
        this.router.navigate(['players']);
      }
    });
  } 

}

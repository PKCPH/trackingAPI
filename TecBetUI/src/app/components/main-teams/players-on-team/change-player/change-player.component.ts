import { MapType } from '@angular/compiler';
import { Component, Type } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Player } from 'src/app/models/player.model';
import { playerTeam } from 'src/app/models/playerTeam.model';
import { Team } from 'src/app/models/teams.model';
import { PlayersService } from 'src/app/services/players.service';
import { TeamsService } from 'src/app/services/teams.service';


@Component({
  selector: 'app-change-player',
  templateUrl: './change-player.component.html',
  styleUrls: ['./change-player.component.css']
})
export class ChangePlayerComponent {

  teamId: string = ''
  teams: Team[] = [];
  selectedTeamArray: Team[] = [];
  selectedTeam: Team = {
    id: '',
    name: '',
    isAvailable: true,
    matches: [],
    availability:'',
    players:[]
  }
  playerDetails: Player = {
    id: '',
    name: '',
    age: 0,
    teams: []
  }

  constructor(private route: ActivatedRoute, private teamsService: TeamsService, private playerService: PlayersService, private router: Router){ }

  ngOnInit(): void {
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('playerId');
        if(id){
          this.getPlayer(id)
        }
      }
    })
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('teamId');
        if(id){
          this.teamId = id
        }
      }
    })
  }

  getPlayer(id: string){
    this.playerService.getPlayer(id)
    .subscribe(
      (response) => {
        this.playerDetails = response;
        console.log(this.playerDetails)
        this.getAllTeams();
      }
    )
  }

  getAllTeams(){
    this.teamsService.getAllTeams()
    .subscribe((teams) => {
        this.teams = teams;
        console.log(teams)
        this.filterTeams();
      }
    )
  }

  filterTeams(){
    this.playerDetails.teams.forEach(element => {
      const team = this.teams.find(t => t.id == element.teamId);
      console.log(team);
      if(team !== undefined){
        this.selectedTeamArray.push(team);
        this.teams.splice(this.teams.indexOf(team),1)
      }
    });
  }

  deletePlayer(id: string){
    this.playerService.deletePlayer(id)
    .subscribe({
      next: (response) => {
        this.router.navigate(['players'])
      }
    })
  }

  removeFromSelectedTeamArray(team: Team){
    this.teams.push(team);
    this.selectedTeamArray.splice(this.selectedTeamArray.indexOf(team),1)
    console.log(this.selectedTeamArray)
  }

  addTeam(){
    if(this.selectedTeamArray.includes(this.selectedTeam) == true) return
    this.selectedTeamArray.push(this.selectedTeam)
    this.teams.splice(this.teams.indexOf(this.selectedTeam),1)
    console.log(this.selectedTeamArray)
  }

  GoBack(){
    this.router.navigate(['teams/players/' + this.teamId])
  }

  updatePlayer(){
    this.selectedTeamArray.forEach(element => {
      const addPlayerTeam: playerTeam = {
        id: '',
        playerId: this.playerDetails.id,
        teamId: element.id
      }
      this.playerDetails.teams.push(addPlayerTeam)
    });

    this.playerService.updatePlayer(this.playerDetails.id, this.playerDetails)
    .subscribe({
      next: (response) => {
        this.router.navigate(['teams/players/' + this.teamId])
      }
    })
  }
}

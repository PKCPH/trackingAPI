import { Component } from '@angular/core';
import { PlayersService } from 'src/app/services/players.service';
import { TeamsService } from 'src/app/services/teams.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Player } from 'src/app/models/player.model';
import { Team } from 'src/app/models/teams.model';
import { playerTeam } from 'src/app/models/playerTeam.model';

@Component({
  selector: 'app-players-to-team',
  templateUrl: './players-to-team.component.html',
  styleUrls: ['./players-to-team.component.css']
})
export class PlayersToTeamComponent {
  /*contains player on the team*/
  playersOnTeam: Player[] = [];
  /*contains player not on the team*/
  playersNotOnTeam: Player[] = [];
  /*contains playerteams that have been removed from the team*/
  playersTeamsRemovedFromTeam: playerTeam[] = [];
  /*contains playerteams that have been added to the team*/
  playersTeamsAddedToTeam: playerTeam[] = [];

  test: number = 0;
  id: string = '';
  selectedTeam: Team = {
    id: '',
    name: '',
    isAvailable: true,
    matches: [],
    availability:'',
    players:[]
  }
  constructor(private route: ActivatedRoute, private teamsService: TeamsService, private playerService: PlayersService, private router: Router){ }
  
  ngOnInit(): void {
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');
        if(id){
          this.id = id
          this.getTeam(id)
        }
      }
    })
  }

  getTeam(id: string){
    this.teamsService.getTeam(id)
    .subscribe(
      (response) => {
        this.selectedTeam = response;
        this.getAllPlayers(id);
      }
    )
  }

  getAllPlayers(id: string){
    this.playerService.getAllPlayers()
    .subscribe((players) => {
        console.log(players)
        players.forEach(element => {
          if(element.teams == null){this.playersNotOnTeam.push(element)}
          else if(element.teams.find(t => t.teamId == id)){ this.playersOnTeam.push(element) }
          else{ this.playersNotOnTeam.push(element) }
        });
        console.log(this.playersOnTeam)
        console.log(this.playersNotOnTeam)
      }
    )
  }

  addPlayerToTeam(id: string){
    var player = this.playersNotOnTeam.find(p => p.id == id)
    if(player){
      this.playersOnTeam.push(player)
      this.playersNotOnTeam.splice(this.playersNotOnTeam.indexOf(player),1)
      
      this.test = 30000
      if(this.selectedTeam.players != null){
        this.test = 30010
        if(!this.selectedTeam.players.some(p => p.playerId == player?.id)){
          this.test = 30100
          const playerTeamm: playerTeam = {
            id: '00000000-0000-0000-0000-000000000000',
            playerId: player.id,
            teamId: this.id
          }
          console.log(playerTeamm)
          this.test = 30110
          this.playersTeamsAddedToTeam.push(playerTeamm)
          this.test = 30120
        }
        else{
          this.test = 30200
          var playerteam = this.playersTeamsRemovedFromTeam.find(p => p.playerId == player?.id)
          this.test = 30210
          if(playerteam){
            this.test = 30220
            var index = this.playersTeamsRemovedFromTeam.indexOf(playerteam)
            this.test = 30230
            this.playersTeamsRemovedFromTeam.splice(index, 1)
            this.test = 30240
          }
        }
      }
      else{
        this.test = 31100
        const playerTeamm: playerTeam = {
          id: '00000000-0000-0000-0000-000000000000',
          playerId: player.id,
          teamId: this.id
        }
        console.log(playerTeamm)
        this.test = 31110
        this.playersTeamsAddedToTeam.push(playerTeamm)
        this.test = 31120
      }
    }
  }

  removePlayerFromTeam(id: string){
    const player = this.playersOnTeam.find(p => p.id == id)
    this.test = 35000
    if(player){
      this.test = 35010
      this.playersNotOnTeam.push(player)
      this.playersOnTeam.splice(this.playersOnTeam.indexOf(player),1)
      this.test = 35020
      if(this.selectedTeam.players.findIndex(p => p.playerId == player.id) > -1){
        this.test = 35100
        this.playersTeamsRemovedFromTeam.push(this.selectedTeam.players[this.selectedTeam.players.findIndex(p => p.playerId == player.id)])
      }
      else{
        this.playersTeamsAddedToTeam.splice(this.playersTeamsAddedToTeam.findIndex(p => p.playerId == player.id),1)
      }
    }
  }

  saveChanges(){  
    this.test = 50000
    var playerTeams: playerTeam[][] = [this.playersTeamsRemovedFromTeam, this.playersTeamsAddedToTeam];
    this.test = 50010
    this.teamsService.changePlayers(playerTeams)
    .subscribe({
      next: (members) => {
        this.router.navigate(['/teams/players/' + this.id])
      }
    })
    this.test = 60000
  }
}

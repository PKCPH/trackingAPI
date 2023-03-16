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
  filteredPlayersOnTeam: Player[] = [];
  /*contains player not on the team*/
  playersNotOnTeam: Player[] = [];
  filteredPlayersNotOnTeam: Player[] = [];
  /*contains playerteams that have been removed from the team*/
  playersTeamsRemovedFromTeam: playerTeam[] = [];
  /*contains playerteams that have been added to the team*/
  playersTeamsAddedToTeam: playerTeam[] = [];

  model = {searchStringOffTeam: "",
  searchStringOnTeam: ""
  }
  id: string = '';
  selectedTeam: Team = {
    id: '',
    name: '',
    isAvailable: true,
    matches: [],
    availability:'',
    players:[],
    score: 0,
    result: 0,
    rating: 0
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
        this.filteredPlayersNotOnTeam = this.playersNotOnTeam
        this.filteredPlayersOnTeam = this.playersOnTeam
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

      if(this.selectedTeam.players != null){
        if(!this.selectedTeam.players.some(p => p.playerId == player?.id)){
          const playerTeamm: playerTeam = {
            id: '00000000-0000-0000-0000-000000000000',
            playerId: player.id,
            teamId: this.id
          }
          console.log(playerTeamm)
          this.playersTeamsAddedToTeam.push(playerTeamm)
        }
        else{
          var playerteam = this.playersTeamsRemovedFromTeam.find(p => p.playerId == player?.id)
          if(playerteam){
            var index = this.playersTeamsRemovedFromTeam.indexOf(playerteam)
            this.playersTeamsRemovedFromTeam.splice(index, 1)
          }
        }
      }
      else{
        const playerTeamm: playerTeam = {
          id: '00000000-0000-0000-0000-000000000000',
          playerId: player.id,
          teamId: this.id
        }
        console.log(playerTeamm)
        this.playersTeamsAddedToTeam.push(playerTeamm)
      }
      this.searchPlayers(true)
    }
  }

  removePlayerFromTeam(id: string){
    const player = this.playersOnTeam.find(p => p.id == id)
    if(player){
      this.playersNotOnTeam.push(player)
      this.playersOnTeam.splice(this.playersOnTeam.indexOf(player),1)
      if(this.selectedTeam.players.findIndex(p => p.playerId == player.id) > -1){
        this.playersTeamsRemovedFromTeam.push(this.selectedTeam.players[this.selectedTeam.players.findIndex(p => p.playerId == player.id)])
      }
      else{
        this.playersTeamsAddedToTeam.splice(this.playersTeamsAddedToTeam.findIndex(p => p.playerId == player.id),1)
      }
      this.searchPlayers(false)
    }
  }

  saveChanges(){
    this.teamsService.getTeam(this.id)
    .subscribe({
      next:(team) => {
        var rating: number = 0
        this.playersOnTeam.forEach(player => {
          rating += player.overall
        });
        rating = rating/this.playersOnTeam.length
        team.rating = Number(rating.toPrecision(4))
      }
    })
    var playerTeams: playerTeam[][] = [this.playersTeamsRemovedFromTeam, this.playersTeamsAddedToTeam];
    this.teamsService.changePlayers(playerTeams)
    .subscribe({
      next: (members) => {
        this.router.navigate(['/teams/players/' + this.id])
      }
    })
  }

  searchPlayers(onTeam: boolean){
    if(onTeam){
      this.filteredPlayersOnTeam = this.playersOnTeam.filter(p => p.name.includes(this.model.searchStringOnTeam))
    }
    else{
      this.filteredPlayersNotOnTeam = this.playersNotOnTeam.filter(p => p.name.includes(this.model.searchStringOffTeam))
    }
  }
}

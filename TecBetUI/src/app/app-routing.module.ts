import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { AddMatchComponent } from './components/main-matches/add-match/add-match.component';
import { EditMatchComponent } from './components/main-matches/edit-match/edit-match.component';
import { MainMatchesComponent } from './components/main-matches/main-matches.component';
import { MainScheduleComponent } from './components/main-schedule/main-schedule.component';
import { AddTeamComponent } from './components/main-teams/add-team/add-team.component';
import { EditTeamComponent } from './components/main-teams/edit-team/edit-team.component';
import { MainTeamsComponent } from './components/main-teams/main-teams.component';
import { LoginComponent } from './components/main-login/login/login.component';
import { AuthguardService } from './services/authguard.service';
import { CustomersComponent } from './components/customers/customers.component';
import { RegisterComponent } from './components/main-login/register/register.component';
import { PlayerListComponent } from './components/main-player/player-list/player-list.component';
import { AddPlayerComponent } from './components/main-player/add-player/add-player.component';
import { EditPlayerComponent } from './components/main-player/edit-player/edit-player.component';
import { PlayersOnTeamComponent } from './components/main-teams/players-on-team/players-on-team.component';
import { UserprofileComponent } from './components/main-login/userprofile/userprofile.component';
import { AdminboardComponent } from './components/main-login/adminboard/adminboard.component';
import { EditUserComponent } from './components/main-login/adminboard/edit-user/edit-user.component';
import { MainHorseracegameComponent } from './components/main-horseracegame/main-horseracegame.component';
import { MatchDetailsComponent } from './components/main-schedule/match-details/match-details.component';
import { UserbetsComponent } from './components/main-login/userprofile/userbets/userbets.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'teams',
    component: MainTeamsComponent
  },
  {
    path: 'teams/add',
    component: AddTeamComponent
  },
  {
    path: 'teams/edit/:id',
    component: EditTeamComponent
  },
  {
    path: 'matches',
    component: MainMatchesComponent
  },
  {
    path: 'matches/add',
    component: AddMatchComponent
  },
  {
    path: 'matches/edit/:id',
    component: EditMatchComponent
  },
  {
    path: 'schedule',
    component: MainScheduleComponent
  },
  {
    path: 'details/:id',
    component: MatchDetailsComponent,
    canActivate: [AuthguardService]
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'adminboard',
    component: AdminboardComponent,
    canActivate: [AuthguardService]
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'players',
    component: PlayerListComponent
  },
  {
    path: 'players/add',
    component: AddPlayerComponent
  },
  {
    path: 'players/edit/:id',
    component: EditPlayerComponent
  },
  {
    path: 'teams/players/:id',
    component: PlayersOnTeamComponent
  },
  {
    path: 'dashboard/:username',
    component: UserprofileComponent,
    canActivate: [AuthguardService]
  },
  {
    path: 'dashboard/:username/mybets',
    component: UserbetsComponent,
    canActivate: [AuthguardService]
  },
  {
    path: 'adminboard/edit-user/:username',
    component: EditUserComponent,
    canActivate: [AuthguardService]
  },
  {
    path: 'animalrace',
    component: MainHorseracegameComponent,
    canActivate: [AuthguardService]
  },
  {
    path: '404',
    component: CustomersComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

import { NgModule } from '@angular/core';
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
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'customers',
    component: CustomersComponent,
    canActivate: [AuthguardService]
  },
  {
    path: 'register',
    component: RegisterComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

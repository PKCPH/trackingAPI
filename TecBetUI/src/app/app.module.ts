import { HttpClientModule } from '@angular/common/http';
import { NgModule, ErrorHandler } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HomeComponent } from './components/home/home.component';
import { MainTeamsComponent } from './components/main-teams/main-teams.component';
import { AddTeamComponent } from './components/main-teams/add-team/add-team.component';
import { EditTeamComponent } from './components/main-teams/edit-team/edit-team.component';
import { BettingComponent } from './components/betting/betting.component';
import { CustomErrorHandlerService } from './services/custom-error-handler.service';
import { MainMatchesComponent } from './components/main-matches/main-matches.component';
import { AddMatchComponent } from './components/main-matches/add-match/add-match.component';
import { EditMatchComponent } from './components/main-matches/edit-match/edit-match.component';
import { MainScheduleComponent } from './components/main-schedule/main-schedule.component';
import { LoginComponent } from './components/main-login/login/login.component';
import { JwtModule } from "@auth0/angular-jwt";
import { CustomersComponent } from './components/customers/customers.component';
import { AuthguardService } from './services/authguard.service';
import { RegisterComponent } from './components/main-login/register/register.component';
import { PlayerListComponent } from './components/main-player/player-list/player-list.component';
import { AddPlayerComponent } from './components/main-player/add-player/add-player.component';
import { EditPlayerComponent } from './components/main-player/edit-player/edit-player.component';
import { PlayersOnTeamComponent } from './components/main-teams/players-on-team/players-on-team.component';

export function tokenGetter() { 
  return localStorage.getItem("jwt"); 
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    MainTeamsComponent,
    AddTeamComponent,
    EditTeamComponent,
    BettingComponent,
    MainMatchesComponent,
    AddMatchComponent,
    EditMatchComponent,
    MainScheduleComponent,
    LoginComponent,
    CustomersComponent,
    RegisterComponent,
    PlayerListComponent,
    AddPlayerComponent,
    EditPlayerComponent,
    PlayersOnTeamComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:5001"],
        disallowedRoutes: []
      } 
    })
  ],
  providers: [
    { provide: ErrorHandler, useClass: CustomErrorHandlerService }, [AuthguardService]
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

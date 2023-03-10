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
import { BlankComponent } from './components/blank/blank.component';
import { AuthguardService } from './services/authguard.service';
import { RegisterComponent } from './components/main-login/register/register.component';
import { PlayerListComponent } from './components/main-player/player-list/player-list.component';
import { AddPlayerComponent } from './components/main-player/add-player/add-player.component';
import { EditPlayerComponent } from './components/main-player/edit-player/edit-player.component';
import { PlayersOnTeamComponent } from './components/main-teams/players-on-team/players-on-team.component';
import { UserprofileComponent } from './components/main-login/userprofile/userprofile.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmboxComponent } from './components/main-login/userprofile/confirmbox/confirmbox.component';
import { ChangepasswordComponent } from './components/main-login/userprofile/changepassword/changepassword.component';
import { AdminboardComponent } from './components/main-login/adminboard/adminboard.component';
import { EditUserComponent } from './components/main-login/adminboard/edit-user/edit-user.component';
import { MainHorseracegameComponent } from './components/main-horseracegame/main-horseracegame.component';
import { MatchDetailsComponent } from './components/main-schedule/match-details/match-details.component';
import { BettingWindowComponent } from './components/betting/betting-window/betting-window.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSliderModule } from '@angular/material/slider';
import { UserbetsComponent } from './components/main-login/userprofile/userbets/userbets.component';
import { PlayersToTeamComponent } from './components/main-teams/players-on-team/players-to-team/players-to-team.component';
import { NewPlayerComponent } from './components/main-teams/players-on-team/new-player/new-player.component';
import { ChangePlayerComponent } from './components/main-teams/players-on-team/change-player/change-player.component';
import {MatIconModule} from '@angular/material/icon';

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
    BlankComponent,
    RegisterComponent,
    PlayerListComponent,
    AddPlayerComponent,
    EditPlayerComponent,
    PlayersOnTeamComponent,
    UserprofileComponent,
    ConfirmboxComponent,
    ChangepasswordComponent,
    AdminboardComponent,
    EditUserComponent,
    MainHorseracegameComponent,
    MatchDetailsComponent,
    BettingWindowComponent,
    UserbetsComponent,
    PlayersOnTeamComponent,
    PlayersToTeamComponent,
    NewPlayerComponent,
    ChangePlayerComponent,
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
    }),
    NgbModule,
    BrowserAnimationsModule,
    MatSliderModule,
    MatIconModule
  ],
  providers: [
    { provide: ErrorHandler, useClass: CustomErrorHandlerService }, [AuthguardService]
  ],
  bootstrap: [AppComponent],

})
export class AppModule { }

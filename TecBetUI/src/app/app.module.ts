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


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    MainTeamsComponent,
    AddTeamComponent,
    EditTeamComponent,
    BettingComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [
    { provide: ErrorHandler }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

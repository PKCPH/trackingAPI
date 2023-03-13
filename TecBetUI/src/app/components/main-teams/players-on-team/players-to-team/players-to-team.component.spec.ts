import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayersToTeamComponent } from './players-to-team.component';

describe('PlayersToTeamComponent', () => {
  let component: PlayersToTeamComponent;
  let fixture: ComponentFixture<PlayersToTeamComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlayersToTeamComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlayersToTeamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

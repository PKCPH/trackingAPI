import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayersOnTeamComponent } from './players-on-team.component';

describe('PlayersOnTeamComponent', () => {
  let component: PlayersOnTeamComponent;
  let fixture: ComponentFixture<PlayersOnTeamComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlayersOnTeamComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlayersOnTeamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

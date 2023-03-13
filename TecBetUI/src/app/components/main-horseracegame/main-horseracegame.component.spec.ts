import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainHorseracegameComponent } from './main-horseracegame.component';

describe('MainHorseracegameComponent', () => {
  let component: MainHorseracegameComponent;
  let fixture: ComponentFixture<MainHorseracegameComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MainHorseracegameComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MainHorseracegameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

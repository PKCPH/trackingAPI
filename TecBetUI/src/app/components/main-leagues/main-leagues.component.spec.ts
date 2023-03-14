import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainLeaguesComponent } from './main-leagues.component';

describe('MainLeaguesComponent', () => {
  let component: MainLeaguesComponent;
  let fixture: ComponentFixture<MainLeaguesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MainLeaguesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MainLeaguesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

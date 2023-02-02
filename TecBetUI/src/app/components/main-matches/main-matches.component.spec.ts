import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainMatchesComponent } from './main-matches.component';

describe('MainMatchesComponent', () => {
  let component: MainMatchesComponent;
  let fixture: ComponentFixture<MainMatchesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MainMatchesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MainMatchesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BettingWindowComponent } from './betting-window.component';

describe('BettingWindowComponent', () => {
  let component: BettingWindowComponent;
  let fixture: ComponentFixture<BettingWindowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BettingWindowComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BettingWindowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

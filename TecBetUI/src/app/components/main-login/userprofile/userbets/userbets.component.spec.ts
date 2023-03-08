import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserbetsComponent } from './userbets.component';

describe('UserbetsComponent', () => {
  let component: UserbetsComponent;
  let fixture: ComponentFixture<UserbetsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserbetsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserbetsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

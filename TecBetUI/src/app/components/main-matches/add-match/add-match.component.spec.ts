import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddMatchComponent } from './add-match.component';

describe('AddMatchComponent', () => {
  let component: AddMatchComponent;
  let fixture: ComponentFixture<AddMatchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddMatchComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddMatchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

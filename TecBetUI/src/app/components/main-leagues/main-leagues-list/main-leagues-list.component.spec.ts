import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainLeaguesListComponent } from './main-leagues-list.component';

describe('MainLeaguesListComponent', () => {
  let component: MainLeaguesListComponent;
  let fixture: ComponentFixture<MainLeaguesListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MainLeaguesListComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(MainLeaguesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

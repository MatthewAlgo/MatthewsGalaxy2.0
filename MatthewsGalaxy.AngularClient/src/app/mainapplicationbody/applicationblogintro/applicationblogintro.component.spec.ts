import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicationblogintroComponent } from './applicationblogintro.component';

describe('ApplicationblogintroComponent', () => {
  let component: ApplicationblogintroComponent;
  let fixture: ComponentFixture<ApplicationblogintroComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ApplicationblogintroComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ApplicationblogintroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

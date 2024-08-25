import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicationfooterComponent } from './applicationfooter.component';

describe('ApplicationfooterComponent', () => {
  let component: ApplicationfooterComponent;
  let fixture: ComponentFixture<ApplicationfooterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ApplicationfooterComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ApplicationfooterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

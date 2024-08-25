import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminpanelcomponentComponent } from './adminpanelcomponent.component';

describe('AdminpanelcomponentComponent', () => {
  let component: AdminpanelcomponentComponent;
  let fixture: ComponentFixture<AdminpanelcomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminpanelcomponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdminpanelcomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

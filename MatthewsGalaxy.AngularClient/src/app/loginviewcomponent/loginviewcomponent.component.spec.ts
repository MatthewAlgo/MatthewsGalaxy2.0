import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginviewcomponentComponent } from './loginviewcomponent.component';

describe('LoginviewcomponentComponent', () => {
  let component: LoginviewcomponentComponent;
  let fixture: ComponentFixture<LoginviewcomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LoginviewcomponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LoginviewcomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

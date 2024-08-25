import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterviewcomponentComponent } from './registerviewcomponent.component';

describe('RegisterviewcomponentComponent', () => {
  let component: RegisterviewcomponentComponent;
  let fixture: ComponentFixture<RegisterviewcomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RegisterviewcomponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RegisterviewcomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

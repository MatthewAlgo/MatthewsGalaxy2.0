import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AboutmecomponentComponent } from './aboutmecomponent.component';

describe('AboutmecomponentComponent', () => {
  let component: AboutmecomponentComponent;
  let fixture: ComponentFixture<AboutmecomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AboutmecomponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AboutmecomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

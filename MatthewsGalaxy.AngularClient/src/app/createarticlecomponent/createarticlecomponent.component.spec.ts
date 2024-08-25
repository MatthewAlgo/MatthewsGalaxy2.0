import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatearticlecomponentComponent } from './createarticlecomponent.component';

describe('CreatearticlecomponentComponent', () => {
  let component: CreatearticlecomponentComponent;
  let fixture: ComponentFixture<CreatearticlecomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreatearticlecomponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreatearticlecomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

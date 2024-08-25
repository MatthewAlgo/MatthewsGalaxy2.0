import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateprojectcomponentComponent } from './createprojectcomponent.component';

describe('CreateprojectcomponentComponent', () => {
  let component: CreateprojectcomponentComponent;
  let fixture: ComponentFixture<CreateprojectcomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreateprojectcomponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateprojectcomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

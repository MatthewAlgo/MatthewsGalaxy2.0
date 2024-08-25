import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectspagecomponentComponent } from './projectspagecomponent.component';

describe('ProjectspagecomponentComponent', () => {
  let component: ProjectspagecomponentComponent;
  let fixture: ComponentFixture<ProjectspagecomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProjectspagecomponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ProjectspagecomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

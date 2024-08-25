import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectcardcomponentComponent } from './projectcardcomponent.component';

describe('ProjectcardcomponentComponent', () => {
  let component: ProjectcardcomponentComponent;
  let fixture: ComponentFixture<ProjectcardcomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProjectcardcomponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ProjectcardcomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

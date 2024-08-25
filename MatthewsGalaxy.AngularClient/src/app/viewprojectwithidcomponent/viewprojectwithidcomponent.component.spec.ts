import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewprojectwithidcomponentComponent } from './viewprojectwithidcomponent.component';

describe('ViewprojectwithidcomponentComponent', () => {
  let component: ViewprojectwithidcomponentComponent;
  let fixture: ComponentFixture<ViewprojectwithidcomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ViewprojectwithidcomponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ViewprojectwithidcomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

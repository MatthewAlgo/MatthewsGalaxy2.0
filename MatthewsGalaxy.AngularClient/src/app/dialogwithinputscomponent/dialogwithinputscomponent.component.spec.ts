import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogwithinputscomponentComponent } from './dialogwithinputscomponent.component';

describe('DialogwithinputscomponentComponent', () => {
  let component: DialogwithinputscomponentComponent;
  let fixture: ComponentFixture<DialogwithinputscomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DialogwithinputscomponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DialogwithinputscomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

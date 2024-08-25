import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewarticlewithidcomponentComponent } from './viewarticlewithidcomponent.component';

describe('ViewarticlewithidcomponentComponent', () => {
  let component: ViewarticlewithidcomponentComponent;
  let fixture: ComponentFixture<ViewarticlewithidcomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ViewarticlewithidcomponentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewarticlewithidcomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

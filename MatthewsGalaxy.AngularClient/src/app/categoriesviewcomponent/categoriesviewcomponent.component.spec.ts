import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoriesviewcomponentComponent } from './categoriesviewcomponent.component';

describe('CategoriesviewcomponentComponent', () => {
  let component: CategoriesviewcomponentComponent;
  let fixture: ComponentFixture<CategoriesviewcomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CategoriesviewcomponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CategoriesviewcomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

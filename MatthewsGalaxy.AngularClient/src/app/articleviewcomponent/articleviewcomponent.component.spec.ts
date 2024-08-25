import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArticleviewcomponentComponent } from './articleviewcomponent.component';

describe('ArticleviewcomponentComponent', () => {
  let component: ArticleviewcomponentComponent;
  let fixture: ComponentFixture<ArticleviewcomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ArticleviewcomponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ArticleviewcomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

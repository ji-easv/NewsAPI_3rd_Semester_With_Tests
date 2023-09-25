import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArticleDisplayComponent } from './article-display.component';

describe('NewsListComponent', () => {
  let component: ArticleDisplayComponent;
  let fixture: ComponentFixture<ArticleDisplayComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ArticleDisplayComponent]
    });
    fixture = TestBed.createComponent(ArticleDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

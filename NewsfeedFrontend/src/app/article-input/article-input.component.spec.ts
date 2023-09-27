import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArticleInputComponent } from './article-input.component';

describe('ArticleInputComponent', () => {
  let component: ArticleInputComponent;
  let fixture: ComponentFixture<ArticleInputComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ArticleInputComponent]
    });
    fixture = TestBed.createComponent(ArticleInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

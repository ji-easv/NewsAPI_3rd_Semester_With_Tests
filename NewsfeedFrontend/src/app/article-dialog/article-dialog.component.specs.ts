import {ComponentFixture, TestBed} from "@angular/core/testing";
import {ArticleDialogComponent} from "./aricle-dialog.component";

describe('ArticleDialogComponent', () => {
  let component: ArticleDialogComponent;
  let fixture: ComponentFixture<ArticleDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ArticleDialogComponent]
    });
    fixture = TestBed.createComponent(ArticleDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

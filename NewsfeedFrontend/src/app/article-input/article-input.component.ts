import {Component, OnInit} from '@angular/core';
import {Article, CreateArticleRequestDto, UpdateArticleRequestDto} from "../Interfaces";
import {ArticleService} from "../ArticleService";
import {DialogService, DynamicDialogConfig, DynamicDialogRef} from "primeng/dynamicdialog";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-article-input',
  template: `
    <form [formGroup]="articleForm">
      <div class="flex flex-column gap-3">
        <input class="mt-1" type="text" pInputText [formControl]="articleForm.controls.headline"
               placeholder="Headline"/>
        <input type="text" pInputText [formControl]="articleForm.controls.author" placeholder="Author"/>
        <input type="text" pInputText [formControl]="articleForm.controls.articleImgUrl"
               placeholder="Article image URL"/>
        <textarea rows="5" pInputTextarea [formControl]="articleForm.controls.body"
                  placeholder="Article body"></textarea>
        <p-button class="align-self-end" (onClick)="saveArticle()">Create</p-button>
      </div>
    </form>
  `,
  styleUrls: ['./article-input.component.css']
})

export class ArticleInputComponent implements OnInit {
  articleId?: number = 0;
  article: Article | undefined;

  articleForm = new FormGroup({
    headline: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(30)]),
    body: new FormControl('', Validators.maxLength(1000)),
    articleImgUrl: new FormControl(''),
    author: new FormControl('', [Validators.required, Validators.pattern('(?:Bob|Rob|Dob|Lob)')])
  });

  constructor(public articleService: ArticleService, public config: DynamicDialogConfig, private dialogRef: DynamicDialogRef) {
  }

  ngOnInit() {
    this.articleId = this.config.data.articleId || 0;
    this.article = this.config.data.article || undefined;

    if (this.article) {
      this.articleForm.controls.headline.setValue(this.article?.headline);
      this.articleForm.controls.body.setValue(this.article?.body);
      this.articleForm.controls.articleImgUrl.setValue(this.article?.articleImgUrl);
      this.articleForm.controls.author.setValue(this.article?.author);
    }
  }

  async saveArticle() {
    if (this.articleForm.valid) {
      try {
        if (this.articleId) {
          await this.articleService.updateArticle(this.articleId, this.articleForm.getRawValue() as UpdateArticleRequestDto)
        } else {
          await this.articleService.createArticle(this.articleForm.getRawValue() as CreateArticleRequestDto);
        }

        this.dialogRef.close();
      } catch (e) {
        if (e instanceof HttpErrorResponse) {
          console.log(e);
        }
      }
    }
  }
}

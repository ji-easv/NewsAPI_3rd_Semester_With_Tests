import {Component, OnInit} from '@angular/core';
import {Article} from "../Interfaces";
import {ArticleService} from "../ArticleService";
import {DynamicDialogConfig} from "primeng/dynamicdialog";
import {FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-article-input',
  template: `
    <form [formGroup]="articleForm">
      <div class="flex flex-column gap-3">
        <input class="mt-1" type="text" pInputText [formControl]="articleForm.controls.headline" placeholder="Headline"/>
        <input type="text" pInputText [formControl]="articleForm.controls.author" placeholder="Author"/>
        <input type="text" pInputText [formControl]="articleForm.controls.articleImgUrl" placeholder="Article image URL"/>
        <textarea rows="5" pInputTextarea [formControl]="articleForm.controls.body" placeholder="Article body"></textarea>
        <p-button (onClick)="createArticle()">Create</p-button>
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
    author: new FormControl('',[Validators.required, Validators.pattern('(?:Bob|Rob|Dob|Lob)')])
  });

  constructor(public articleService: ArticleService, public config: DynamicDialogConfig) {
  }

  ngOnInit() {
    this.articleId = this.config.data.articleId || 0;

    if (this.articleId) {
      this.articleService.getArticle(this.articleId).then(r => {
        this.article = r;
      });
    }
  }

  createArticle() {
    if (this.articleForm.valid) {
      this.articleService.createArticle({
        headline: this.articleForm.controls.headline.value,
        body: this.articleForm.controls.body.value,
        articleImgUrl: this.articleForm.controls.articleImgUrl.value,
        author: this.articleForm.controls.author.value
      });
    }
  }
}

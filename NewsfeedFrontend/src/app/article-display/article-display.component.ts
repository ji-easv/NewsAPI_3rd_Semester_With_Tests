import {Component, Input, OnInit} from '@angular/core';
import {ArticleService} from "../ArticleService";
import {Article} from "../Interfaces";
import {DynamicDialogConfig} from "primeng/dynamicdialog";

@Component({
  selector: 'app-article-display',
  template: `
    <h2>{{article?.headline || "Headline"}}</h2>
    <h4>By {{article?.author || "Author"}}</h4>
    <img style="width: 50%; height: auto" [src]="article?.articleImgUrl" [alt]="article?.headline">
    <p>{{article?.body}}</p>
  `,
  styleUrls: ['./article-display.component.css']
})

export class ArticleDisplayComponent implements OnInit {
  articleId: number = 0;
  article: Article | undefined;

  constructor(public articleService: ArticleService, public config: DynamicDialogConfig) {
  }

  ngOnInit() {
    this.articleId = this.config.data.articleId;

    this.articleService.getArticle(this.articleId).then((article) => {
      this.article = article;
    });
  }
}

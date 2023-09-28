import { Component, OnDestroy } from '@angular/core';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import {ArticleService} from "../ArticleService";
import {Article} from "../Interfaces";
import {ArticleDisplayComponent} from "../article-display/article-display.component";

@Component({
  template: `<p>{{article?.headline}}</p>`,
  providers: [DialogService, ArticleService]
})
export class ArticleDialogComponent implements OnDestroy {
  article: Article | undefined;
  ref: DynamicDialogRef | undefined;

  constructor(private dialogService: DialogService, private articleService : ArticleService) {
    //TODO: figure out how to call this from the parent component

  }

  ngOnDestroy(): void {
    if (this.ref) {
      this.ref.close();
    }
  }

  async show(articleId: number) {
    this.article = await this.articleService.getArticle(articleId);
    this.ref = this.dialogService.open( ArticleDisplayComponent, { header: 'Article', width: '70%', contentStyle: {'max-height': '500px', 'overflow': 'auto'}, baseZIndex: 10000 });
  }
}

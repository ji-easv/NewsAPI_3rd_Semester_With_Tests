import { Component } from '@angular/core';
import {ArticleService} from "../ArticleService";

@Component({
  selector: 'app-article-display',
  template: `
    <p-dataView [value]="articleService.articles">
      <ng-template let-article pTemplate="listItem">
        <div class="col-12">
          <p>IDK what im doing</p>
        </div>
      </ng-template>
    </p-dataView>
    `,
  styleUrls: ['./article-display.component.css']
})
export class ArticleDisplayComponent {
  constructor(public articleService: ArticleService) {
    articleService.getArticles().then(r => console.log(r));
  }
}

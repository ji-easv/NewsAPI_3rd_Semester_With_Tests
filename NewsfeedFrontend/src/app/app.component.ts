import { Component } from '@angular/core';
import {ArticleService} from "./ArticleService";

@Component({
  selector: 'app-root',
  template: `
    <p-dataView [value]="articleService.articles">
      <ng-template let-article pTemplate="listItem">
        <div class="col-12">
          <p>{{article.headline}}</p>
        </div>
      </ng-template>
    </p-dataView>`,
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'NewsfeedFrontend';

  constructor(public articleService: ArticleService) {

  }
}

import {Injectable} from "@angular/core";
import {Article, NewsFeedItem} from "./Interfaces";
import {HttpClient} from "@angular/common/http";
import {firstValueFrom} from "rxjs";

@Injectable({
  providedIn: 'root'
})

export class ArticleService {
  articles: NewsFeedItem[] = [];

  constructor(private http: HttpClient) {
    this.getArticles().then(r => console.log(r));
  }

  async getArticles() {
    const call = this.http.get<NewsFeedItem[]>("http://localhost:5000/api/feed");
    this.articles = await firstValueFrom<NewsFeedItem[]>(call);
  }

  async getArticle(id: number) {
    const call = this.http.get<Article>("http://localhost:5000/api/articles/" + id);
    const article = await firstValueFrom<Article>(call);
    console.log(article);
    return article;
  }
}

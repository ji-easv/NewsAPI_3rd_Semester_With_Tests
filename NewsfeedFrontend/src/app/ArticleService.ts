import {Injectable} from "@angular/core";
import {NewsFeedItem} from "./Interfaces";
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
}

import {Injectable} from "@angular/core";
import {Article, CreateArticleRequestDto, NewsFeedItem} from "./Interfaces";
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
    return this.articles;
  }

  async getArticle(id: number) {
    const call = this.http.get<Article>("http://localhost:5000/api/articles/" + id);
    const article = await firstValueFrom<Article>(call);
    console.log(article);
    return article;
  }

  deleteArticle(articleId: number) {
    const call = this.http.delete("http://localhost:5000/api/articles/" + articleId);
    const response = firstValueFrom(call);
    response.then(r => {
        this.getArticles();
      }
    );
  }

  createArticle(article: CreateArticleRequestDto) {
    const call = this.http.post("http://localhost:5000/api/articles", article);
    const response = firstValueFrom(call);
    response.then(r => {
        this.getArticles();
      }
    );
  }

  editArticle(articleId: number) {

  }
}

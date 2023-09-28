import {Injectable} from "@angular/core";
import {Article, CreateArticleRequestDto, NewsFeedItem, UpdateArticleRequestDto} from "./Interfaces";
import {HttpClient} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})

export class ArticleService {
  articles: NewsFeedItem[] = [];

  constructor(private http: HttpClient) {
    this.getArticles().then(r => console.log(r));
  }

  async getArticles() {
    const call = this.http.get<NewsFeedItem[]>(environment.baseUrl + "/api/feed");
    this.articles = await firstValueFrom<NewsFeedItem[]>(call);
    return this.articles;
  }

  async getArticle(id: number) {
    const call = this.http.get<Article>(environment.baseUrl + "/api/feed" + id);
    const article = await firstValueFrom<Article>(call);
    console.log(article);
    return article;
  }

  deleteArticle(articleId: number) {
    const call = this.http.delete(environment.baseUrl + "/api/feed" + articleId);
    const response = firstValueFrom(call);
    response.then(r => {
        this.getArticles();
      }
    );
  }

  createArticle(article: CreateArticleRequestDto) {
    const call = this.http.post<Article>(environment.baseUrl + "/api/feed", article);
    const response = firstValueFrom<Article>(call);
    response.then(r => {
        this.getArticles();
      }
    );
  }

  updateArticle(articleId: number, article: UpdateArticleRequestDto) {
    const call = this.http.put<Article>(environment.baseUrl + "/api/feed" + articleId, article);
    const response = firstValueFrom<Article>(call);
    response.then(r => {
        this.getArticles();
      }
    );
  }
}

export interface Article {
  articleId: number | null;
  headline: string | null;
  body: string | null;
  articleImgUrl: string | null;
  author: string | null;
}

export interface CreateArticleRequestDto {
  headline: string | null;
  body: string | null;
  articleImgUrl: string | null;
  author: string | null;
}

export interface NewsFeedItem {
  articleId: number | null;
  headline: string | null;
  body: string | null;
  articleImgUrl: string | null;
}

export interface SearchArticleItem {
  articleId: number | null;
  body: string | null;
  articleImgUrl: string | null;
}

export interface UpdateArticleRequestDto {
  headline: string | null;
  body: string | null;
  articleImgUrl: string | null;
  author: string | null;
}

export class ResponseDto<T> {
  data?: T;
  message?: string;
  success?: boolean;
}

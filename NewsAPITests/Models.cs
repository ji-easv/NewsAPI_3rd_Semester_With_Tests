namespace Tests;

public class Article
{
    public int ArticleId { get; set; }
    public string Headline { get; set; }
    public string Body { get; set; }
    public string Author { get; set; }
    public string ArticleImgUrl { get; set; }
}


public class SearchArticleItem
{
    public string Headline { get; set; }
    public int ArticleId { get; set; }
    public string Author { get; set; }

}
public class NewsFeedItem
{
    public string Headline { get; set; }
    public int ArticleId { get; set; }
    public string ArticleImgUrl { get; set; }
    public string Body { get; set; }
}
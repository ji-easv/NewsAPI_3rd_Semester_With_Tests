namespace Infrastructure.Models;

public class NewsFeedItem
{
    public int? ArticleId { get; set; }
    public string? Headline { get; set; }
    public string? Body { get; set; }
    public string? ArticleImgUrl { get; set; }
}
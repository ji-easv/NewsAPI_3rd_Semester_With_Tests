namespace Infrastructure.Models;

public class Article
{
    public int? ArticleId { get; set; }
    public string? Headline { get; set; }
    public string? Body { get; set; }
    public string? ArticleImgUrl { get; set; }
    public string? Author { get; set; }
}
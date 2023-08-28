using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class NewsFeedItem
{
    public int? ArticleId { get; set; }
    public string? Headline { get; set; }
    public string? Body { get; set; } // Less than 51 chars
    public string? ArticleImgUrl { get; set; }
}
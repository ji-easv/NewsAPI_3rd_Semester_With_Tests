using System.ComponentModel.DataAnnotations;

namespace Core.Model;

public class CreateArticleRequestDto
{
    [MinLength(5)]
    [MaxLength(30)]
    public string? Headline { get; set; }
    
    [MaxLength(1000)]
    public string? Body { get; set; }
    
    public string? ArticleImageUrl { get; set; }
    public string? Author { get; set; }
}
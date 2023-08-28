using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Models;

public class CreateArticleRequestDto
{
    [MinLength(5)]
    [MaxLength(30)]
    public string? Headline { get; set; }
    
    [MaxLength(1000)]
    public string? Body { get; set; }
    
    [NotNull]
    public string? ArticleImgUrl { get; set; }
    public string? Author { get; set; }
}
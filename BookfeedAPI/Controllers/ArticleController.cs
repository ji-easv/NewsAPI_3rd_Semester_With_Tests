using Core.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookfeedAPI.Controllers;

[ApiController]
[Route("api")]
public class ArticleController : ControllerBase
{
    private readonly ArticleService _articleService;

    public ArticleController(ArticleService articleService)
    {
        _articleService = articleService;
    }
    
    [HttpGet("articles/{id}")]
    public Article Get(int id)
    {
        return _articleService.Get(id);
    }
    
    [HttpGet("feed")]
    public IEnumerable<NewsFeedItem> GetFeed()
    {
        return _articleService.GetFeed();
    }
    
    [HttpGet("articles")]
    public IEnumerable<SearchArticleItem> Search([FromQuery(Name = "searchTerm")] string query,
        [FromQuery(Name = "pageSize")] int pageSize)
    {
        return _articleService.Search(query, 1, pageSize);
    }
    
    [HttpPost("articles")]
    public Article Create([FromBody]CreateArticleRequestDto articleDto)
    {
        try
        {
            return _articleService.Create(articleDto);
        }
        catch (ArgumentException exception)
        {
            HttpContext.Response.StatusCode = 400;
            HttpContext.Response.WriteAsJsonAsync(exception.Message);
            return null;
        }
        catch (Exception exception)
        {
            HttpContext.Response.StatusCode = 500;
            HttpContext.Response.WriteAsJsonAsync(exception.Message);
            return null;
        }
    }
    
    [HttpPut("articles/{id}")]
    public Article Update(int id, [FromBody] UpdateArticleRequestDto articleDto)
    {
        return _articleService.Update(id, articleDto);
    }
    
    [HttpDelete("articles/{id}")]
    public void Delete(int id)
    {
        _articleService.Delete(id);
    }
}
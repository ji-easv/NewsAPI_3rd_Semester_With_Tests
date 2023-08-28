using Core.Model;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookfeedAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticleController : ControllerBase
{
    private readonly ArticleService _articleService;

    public ArticleController(ArticleService articleService)
    {
        _articleService = articleService;
    }

    [HttpGet]
    public List<Article> Get()
    {
        return _articleService.Get();
    }
    
    [HttpGet("{id}")]
    public Article Get(int id)
    {
        return _articleService.Get(id);
    }
    
    [HttpPost]
    public Article Create(Article article)
    {
        return _articleService.Create(article);
    }
    
    [HttpPut("{id}")]
    public Article Update(int id, Article article)
    {
        return _articleService.Update(id, article);
    }
    
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        _articleService.Delete(id);
    }
}
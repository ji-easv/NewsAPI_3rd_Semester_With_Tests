using Core.Model;
using Infrastructure;

namespace Core.Services;

public class ArticleService
{
    private readonly ArticleRepository _articleRepository;

    public ArticleService(ArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }
    
    public List<Article> Get()
    {
        return _articleRepository.Get();
    }
    
    public Article Get(int id)
    {
        throw new NotImplementedException();
    }
    
    public Article Create(Article article)
    {
        throw new NotImplementedException();
    }
    
    public Article Update(int id, Article article)
    {
        throw new NotImplementedException();
    }
    
    public void Delete(int id)
    {
        throw new NotImplementedException();
    }
}
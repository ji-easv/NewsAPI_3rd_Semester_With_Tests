using Infrastructure;
using Infrastructure.Models;

namespace Core.Services;

public class ArticleService
{
    private readonly ArticleRepository _articleRepository;
    private readonly List<string> _validAuthors = new() {"Rob", "Bob", "Dob", "Lob"};

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
        return _articleRepository.Get(id);
    }
    
    public Article Create(CreateArticleRequestDto articleDto)
    {

        if (_validAuthors.Contains(articleDto.Author))
        {
            try
            {
                return _articleRepository.Create(articleDto);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw new Exception("Could not create article");
            }
        }
        throw new ArgumentException("Author is not valid");
    }
    
    public Article Update(int id, UpdateArticleRequestDto articleDto)
    {
        return _articleRepository.Update(id, articleDto);
    }
    
    public void Delete(int id)
    {
        _articleRepository.Delete(id);
    }
}
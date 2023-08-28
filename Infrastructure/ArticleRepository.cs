using Core.Model;
using Npgsql;

namespace Infrastructure;

public class ArticleRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public ArticleRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public List<Article> Get()
    {
        throw new NotImplementedException();
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
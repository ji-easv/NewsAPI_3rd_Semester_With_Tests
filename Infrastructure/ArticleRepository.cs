using Dapper;
using Infrastructure.Models;
using Npgsql;

namespace Infrastructure;

public class ArticleRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public ArticleRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public IEnumerable<Article> Get()
    {
        var sql = @"SELECT * FROM news.articles";
        using var connection = _dataSource.OpenConnection();
        return connection.Query<Article>(sql).ToList();
    }
    
    public Article Get(int id)
    {
        var sql = @"SELECT * FROM news.articles WHERE articleid = @articleId";
        using var connection = _dataSource.OpenConnection();
        return connection.QueryFirst<Article>(sql, new {articleId = id});
    }
    
    public IEnumerable<NewsFeedItem> GetFeed()
    {
        var sql = @"SELECT articleid, headline, LEFT(body, 50) AS body, articleimgurl FROM news.articles";
        using var connection = _dataSource.OpenConnection();
        return connection.Query<NewsFeedItem>(sql);
    }
    
    public Article Create(CreateArticleRequestDto articleDto)
    {
        var sql = @"INSERT INTO news.articles (headline, author, body, articleimgurl) 
                        VALUES (@headline, @author, @body, 
                                @articleimgurl) RETURNING *";
        using var connection = _dataSource.OpenConnection();
        return connection.QueryFirst<Article>(sql, new {headline = articleDto.Headline, 
            author = articleDto.Author, body = articleDto.Body, articleimgurl = articleDto.ArticleImgUrl});
    }
    
    public Article Update(int id, UpdateArticleRequestDto article)
    {
        var sql = @"UPDATE news.articles SET headline = @headline, author = @author,
                        body = @body, articleimgurl = @articleimgurl WHERE articleid = @articleId
                        RETURNING *";
        using var connection = _dataSource.OpenConnection();
        return connection.QueryFirst<Article>(sql, new {headline = article.Headline, 
            author = article.Author, body = article.Body, articleimgurl = article.ArticleImgUrl, articleId = id});
    }
    
    public void Delete(int id)
    {
        var sql = "DELETE FROM news.articles WHERE articleid = @articleId";
        using var connection = _dataSource.OpenConnection();
        connection.Execute(sql, new {articleId = id});
    }
}
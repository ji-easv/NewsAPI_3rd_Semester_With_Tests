using System.Data;
using Dapper;
using Infrastructure.Models;
using Npgsql;

namespace Infrastructure;

public class ArticleRepository
{
    private readonly IDbConnection _connection;
    private readonly string _databaseSchema;

    public ArticleRepository(IDbConnection connection)
    {
        _connection = connection;
        _databaseSchema = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" 
            ?  "tests"
            :  "news";
    }
    
    public Article Get(int id)
    {
        var sql = @$"SELECT * FROM {_databaseSchema}.articles WHERE articleid = @articleId";
        return _connection.QueryFirst<Article>(sql, new {articleId = id});
    }
    
    public IEnumerable<NewsFeedItem> GetFeed()
    {
        var sql = $@"SELECT articleid, headline, LEFT(body, 50) AS body, articleimgurl FROM {_databaseSchema}.articles";
        return _connection.Query<NewsFeedItem>(sql);
    }
    
    public IEnumerable<SearchArticleItem> Search(string query, int page, int pageSize)
    {
        var sql = $@"SELECT * FROM {_databaseSchema}.articles WHERE headline ILIKE @query OR body ILIKE @query 
                    LIMIT @pageSize OFFSET @page * @pageSize";
        return _connection.Query<SearchArticleItem>(sql, new {query = $"%{query}%", page = page-1, pageSize});
    }
    
    public Article Create(CreateArticleRequestDto articleDto)
    {
        Console.WriteLine(_databaseSchema);
        var sql = $@"INSERT INTO {_databaseSchema}.articles (headline, author, body, articleimgurl) 
                        VALUES (@headline, @author, @body, 
                                @articleimgurl) RETURNING *";
        return _connection.QueryFirst<Article>(sql, new {headline = articleDto.Headline, 
            author = articleDto.Author, body = articleDto.Body, articleimgurl = articleDto.ArticleImgUrl});
    }
    
    public Article Update(int id, UpdateArticleRequestDto article)
    {
        var sql = $@"UPDATE {_databaseSchema}.articles SET headline = @headline, author = @author,
                        body = @body, articleimgurl = @articleimgurl WHERE articleid = @articleId
                        RETURNING *";
        return _connection.QueryFirst<Article>(sql, new {headline = article.Headline, 
            author = article.Author, body = article.Body, articleimgurl = article.ArticleImgUrl, articleId = id});
    }
    
    public void Delete(int id)
    {
        var sql = $"DELETE FROM {_databaseSchema}.articles WHERE articleid = @articleId";
        _connection.Execute(sql, new {articleId = id});
    }
}
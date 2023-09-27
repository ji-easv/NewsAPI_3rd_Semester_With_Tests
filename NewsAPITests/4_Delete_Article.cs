using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace Tests;

public class DeleteArticle
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
    }
    
    [Test]
    public async Task GetFullArticleTest()
    {
        Helper.TriggerRebuild();

        var article = new Article()
        {
            ArticleId = 1,
            Headline = "hello world",
            Body = "Mock bodu",
            Author = "Rob",
            ArticleImgUrl = "https://images.unsplash.com/photo-1504711434969-e33886168f5c?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80"
        };
        var sql = $@" 
            insert into news.articles (headline, body, author, articleimgurl) VALUES (@headline, @body, @author, @articleimgurl);
            ";
        using (var conn = Helper.DataSource.OpenConnection())
        {
            conn.Execute(sql, article);
        }

        var url = "http://localhost:5000/api/articles/1";
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.DeleteAsync(url);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());

        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }

        using (new AssertionScope())
        {
            using (var conn = Helper.DataSource.OpenConnection())
            {
                (conn.ExecuteScalar<int>($"SELECT COUNT(*) FROM news.articles WHERE articleid = 1;") == 0)
                    .Should()
                    .BeTrue();
            }
            (await Helper.IsCorsFullyEnabledAsync(url)).Should().BeTrue();
            response.IsSuccessStatusCode.Should().BeTrue();
        }
    }
}
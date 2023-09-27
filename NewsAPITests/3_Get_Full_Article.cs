using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests;

public class GetFullArticle
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
            Body = "Mock body",
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
            response = await _httpClient.GetAsync(url);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());

        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }

        var content = await response.Content.ReadAsStringAsync();
        Article actual;
        try
        {
            actual = JsonConvert.DeserializeObject<Article>(content) ??
                       throw new InvalidOperationException();
        }
        catch (Exception e)
        {
            throw new Exception(Helper.BadResponseBody(await response.Content.ReadAsStringAsync()), e);
        }

        using (new AssertionScope())
        {
            actual.Body.Should().BeEquivalentTo(article.Body);
            response.IsSuccessStatusCode.Should().BeTrue();
            actual.Author.Should().BeEquivalentTo(article.Author);
            actual.ArticleId.Should().Be(1);
            (await Helper.IsCorsFullyEnabledAsync(url)).Should().BeTrue();

        }
    }
}
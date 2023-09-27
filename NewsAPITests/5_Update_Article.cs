using System.Net.Http.Json;
using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests;

public class UpdateArticle
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
    }


    [Test]
    public async Task SuccessfullyUpdateArticle()
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

        var url = "http://localhost:5000/api/articles/" + 1;
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PutAsJsonAsync(url, article);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }

        Article responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<Article>(
                await response.Content.ReadAsStringAsync()) ?? throw new InvalidOperationException();
        }
        catch (Exception e)
        {
            throw new Exception(Helper.BadResponseBody(await response.Content.ReadAsStringAsync()), e);
        }

        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            (await Helper.IsCorsFullyEnabledAsync(url)).Should().BeTrue();
            responseObject.Should().BeEquivalentTo(article, Helper.MyBecause(responseObject, article));
        }
    }

    [TestCase("Mock headline", "Mock body", "Author who doesn't exist", "https://images.unsplash.com/photo-1504711434969-e33886168f5c?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80", false)]
    [TestCase("", "Mock body", "Rob", "https://images.unsplash.com/photo-1504711434969-e33886168f5c?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80", false)]
    [TestCase("asdlkjsadlksajdlksajdlksajdlksadjldskajasdkl", "Mock body", "Rob", "https://images.unsplash.com/photo-1504711434969-e33886168f5c?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80", false)]
    [TestCase("Mock Headline", "Mock body", "Rob", null, false)]
    public async Task UpdateShouldFailDueToDataValidation(string headline, string body, string author,
        string articleImgUrl, bool testPassing)
    {
        var article = new Article()
        {
            Headline = headline,
            Author = author,
            Body = body,
            ArticleId = 1,
            ArticleImgUrl = articleImgUrl
        };
        var url = "http://localhost:5000/api/articles/" + 1;
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PutAsJsonAsync(url, article);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }
        

        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeFalse();
        }
    }
}
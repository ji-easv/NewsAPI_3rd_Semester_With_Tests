using System.Net.Http.Json;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using NUnit.Framework;
using Tests;

namespace NewsAPITests;

public class CreateArticle
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
    }


    [Test]
    public async Task ShouldSuccessfullyCreateBook()
    {
        Helper.TriggerRebuild();
        var article = new Article()
        {
            Headline = "Mock headline",
            Author = "Rob",
            Body = "bla bla bla",
            ArticleId = 1,
            ArticleImgUrl = "https://images.unsplash.com/photo-1504711434969-e33886168f5c?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80"
        };
        var url = "http://localhost:5000/api/articles";
        
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync(url, article);
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
            (await Helper.IsCorsFullyEnabledAsync(url)).Should().BeTrue();
            response.IsSuccessStatusCode.Should().BeTrue();
            responseObject.Should().BeEquivalentTo(article, Helper.MyBecause(responseObject, article));
        }
    }

    [TestCase("Mock headline", "Mock body", "Author who doesn't exist", "url")]
    [TestCase("", "Mock body", "Rob", "url")]
    [TestCase("asdlkjsadlksajdlksajdlksajdlksadjldskajasdkl", "Mock body", "Rob", "url")]
    [TestCase("Mock Headline", "Mock body", "Rob", null)]
    public async Task ShouldFailDueToDataValidation(string headline, string body, string author, string articleImgUrl)
    {
        var article = new Article()
        {
            Headline = headline,
            Author = author,
            Body = body,
            ArticleId = 1,
            ArticleImgUrl = articleImgUrl
        };

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync("http://localhost:5000/api/articles", article);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }
        
        response.IsSuccessStatusCode.Should().BeFalse();
    }
}
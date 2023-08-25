using System.Net.Http.Json;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests;

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
            ArticleImgUrl = "https://someimg/img.jpg"
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
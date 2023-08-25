using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests;

public class GetArticleFeed
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
    }
    
    [Test]
    public async Task GetArticleFeedTest()
    {
        Helper.TriggerRebuild();
        var expected = new List<object>();
        for (var i = 1; i < 10; i++)
        {
            var article = new Article()
            {
                ArticleId = i,
                Headline = "hello world",
                Body = "Mock super long title above 50 chars which the actual title should crop down to max 50 characters",
                Author = "Rob",
                ArticleImgUrl = "someurl"
            };
            expected.Add(article);
            var sql = $@" 
            insert into news.articles (headline, body, author, articleimgurl) VALUES (@headline, @body, @author, @articleimgurl);
            ";
            using (var conn = Helper.DataSource.OpenConnection())
            {
                conn.Execute(sql, article);
            }
        }

        var url = "http://localhost:5000/api/feed";
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


        IEnumerable<NewsFeedItem> articles;
        try
        {
            articles = JsonConvert.DeserializeObject<IEnumerable<NewsFeedItem>>(await response.Content.ReadAsStringAsync()) ??
                       throw new InvalidOperationException();
        }
        catch (Exception e)
        {
            throw new Exception(Helper.BadResponseBody(await response.Content.ReadAsStringAsync()), e);
        }

        using (new AssertionScope())
        {
            foreach (var article in articles)
            {
                //If you want to be super strict, you can also check that the response.content does not include an author
                article.Body.Length.Should().BeLessThan(51);
                response.IsSuccessStatusCode.Should().BeTrue();
                article.ArticleId.Should().BeGreaterThan(0);
                (await Helper.IsCorsFullyEnabledAsync(url)).Should().BeTrue();

            }
        }
    }
}
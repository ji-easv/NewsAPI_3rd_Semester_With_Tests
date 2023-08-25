using Bogus;
using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests;

public class SearchArticles
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
    }


    [TestCase("qqq asd", 5, 5)]
    [TestCase("dsklfj", 5, 0)]
    public async Task SuccessfullArticleSearch(string searchterm, int pageSize, int resultSize)
    {
        Helper.TriggerRebuild();
        var expected = new List<object>();
        for (var i = 1; i < 10; i++)
        {
            var article = new Article()
            {
                ArticleId = i,
                Headline = new Faker().Random.Words(2),
                Body = "asdasdl qqq asdlkjasdlk",
                Author = new Faker().Random.Word(),
                ArticleImgUrl = new Faker().Random.Word()
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

        var url = $"http://localhost:5000/api/articles?searchTerm={searchterm}&pagesize={pageSize}";
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
        IEnumerable<SearchArticleItem> articles;
        try
        {
            articles = JsonConvert.DeserializeObject<IEnumerable<SearchArticleItem>>(content) ??
                       throw new InvalidOperationException();
        }
        catch (Exception e)
        {
            throw new Exception(Helper.NoResponseMessage, e);
        }

        using (new AssertionScope())
        {
            articles.Count().Should().Be(resultSize);
            response.IsSuccessStatusCode.Should().BeTrue();
            (await Helper.IsCorsFullyEnabledAsync(url)).Should().BeTrue();
        }
    }

    [TestCase("qq", 5)]
    [TestCase("dsklfj", -5)]
    public async Task ArticleSearchFailBecauseOfDataValidation(string searchterm, int pageSize)
    {
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(
                $"http://localhost:5000/api/articles?searchTerm={searchterm}&pagesize={pageSize}");
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
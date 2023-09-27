using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace NewsAPITests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class FrontendTests : PageTest
{
    [SetUp]
    public void Setup()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
    }
    
    [Test]
    public async Task HomepageHasProperTitle()
    {
        await Page.GotoAsync("http://localhost:4200");

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync("Newsfeed");
    }
    
    [Test]
    public async Task CreateArticleSuccess()
    {
        await Page.GotoAsync("http://localhost:4200");
        
        await Page.ClickAsync("text=Create Article");
        await Page.FillAsync("input[formcontrolname=title]", "Test Title");
        await Page.FillAsync("input[formcontrolname=body]", "Test Body");
        await Page.FillAsync("input[formcontrolname=author]", "Rob");
        
        await Page.ClickAsync("text=Create");
        
        
    }
}
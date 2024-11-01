using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace PlaywrightApiTests;

[TestClass]
public class TestGitHubAPI : PlaywrightTest
{
    static string? API_TOKEN = Environment.GetEnvironmentVariable("GITHUB_API_TOKEN");

    private IAPIRequestContext Request = null!;

    [TestInitialize]
    public async Task SetUpAPITesting()
    {
        await CreateAPIRequestContext();
    }

    private async Task CreateAPIRequestContext()
    {
        var headers = new Dictionary<string, string>();
        // We set this header per GitHub guidelines.
        headers.Add("Accept", "application/vnd.github.v3+json");
        // Add authorization token to all requests.
        // Assuming personal access token available in the environment.
        headers.Add("Authorization", "token " + API_TOKEN);

        Request = await this.Playwright.APIRequest.NewContextAsync(new() {
            // All requests we send go to this API endpoint.
            BaseURL = "https://api.github.com",
            ExtraHTTPHeaders = headers,
        });
    }

    [TestCleanup]
    public async Task TearDownAPITesting()
    {
        await Request.DisposeAsync();
    }
}
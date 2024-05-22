using System.Net;
using System.Text.Json.Nodes;
using Ardalis.HttpClientTestExtensions;
using Infrastructure.Dto;
using Infrastructure.Models.Dto;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickFixAPI.Data;
using QuickFixAPI.Models;
using QuickFixAPITest.Logger;
using Xunit.Abstractions;

namespace QuickFixAPITest.Tests;



public class TestGetAllFixesEndpoint
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ILogger _logger;
    private readonly HttpClient _client;

    public TestGetAllFixesEndpoint(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _logger = XUnitLogger.CreateLogger<TestGetAllFixesEndpoint>(_testOutputHelper);
        var webApp = new WebApiApplication(_testOutputHelper);
        //webApp.ClientOptions.BaseAddress =new Uri("https://localhost:7123/api/Fix");
		_client = webApp.CreateClient();
    }

    [Fact]
    public async Task GetFixes()
    {
        var response = await _client.GetAsync("/api/Fix");

        _logger.LogInformation($"URL: {response.RequestMessage.RequestUri.AbsoluteUri}");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        /* Static casting
        var result = await _client.GetAndDeserialize<ResponseDto>(Routes.Fixes.List);
        Assert.NotNull(result);
        Assert.Equal(SeedData.Fixes().Count, JsonConvert.DeserializeObject<IEnumerable<FixDto>>(result.Result.ToString()).Count());
        */

        //dynamic
        var jsonString = await response.Content.ReadAsStringAsync();
        var jsonResponse = JObject.Parse(jsonString);

        Assert.NotNull(jsonResponse);

        // Assuming that 'Result' is a property in the JSON response that contains the list of fixes
        var fixes = jsonResponse["result"]?.ToObject<IEnumerable<JObject>>();
        Assert.NotNull(fixes);

        // Assuming you have a method SeedData.Fixes() that provides a list of expected fixes
        Assert.Equal(SeedData.Fixes().Count, fixes.Count());
    }

}



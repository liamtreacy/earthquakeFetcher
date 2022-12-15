using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using TechTalk.SpecFlow.Infrastructure;
using Xunit;

namespace tests.Steps;

[Binding]
public sealed class CalculatorStepDefinitions
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly ScenarioContext _scenarioContext;
    private const string BaseAddress = "http://localhost/";
    private readonly ISpecFlowOutputHelper _outputHelper;
    public WebApplicationFactory<Program> Factory { get; }
    public HttpClient Client { get; set; } = null!;
    private HttpResponseMessage Response { get; set; } = null!;

    public CalculatorStepDefinitions(ScenarioContext scenarioContext, WebApplicationFactory<Program> factory, ISpecFlowOutputHelper outputHelper)
    {
        _scenarioContext = scenarioContext;
        Factory = factory;
        _outputHelper = outputHelper;
    }

    [Given("the stored data is cleared")]
    public async void ClearData()
    {
         Response = await Client.DeleteAsync("");
    }
    
    [Given("I am a client")]
    public void GivenIAmAClient()
    {
        Client = Factory.CreateDefaultClient(new Uri(BaseAddress));
    }
    
    [When(@"I make a GET request to '(.*)'")]
    public async Task WhenIMakeAgetRequestWithIdTo(string endpoint)
    {
        Response = await Client.GetAsync($"{endpoint}");
    }
    
    [Then(@"the response status code is '(.*)'")]
    public void ThenTheResponseStatusCodeIs(int statusCode)
    {
        var expected = (HttpStatusCode)statusCode;
        Assert.Equal(expected, Response.StatusCode);
    }

    [Then(@"the response json should not be  empty")]
    public async Task ThenTheResponseDataShouldBe()
    {
        var response = await Response.Content.ReadAsStringAsync();
        //_outputHelper.WriteLine("LIAMLIAM");
        Assert.NotEmpty(response);
    }
}
using System.Diagnostics;

namespace ExternalDataRetrieverSpecFlowTests.Steps;

[Binding]
public sealed class ExternalDataRetrieverServiceStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;

    public ExternalDataRetrieverServiceStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }
    
    [After]
    public void tidyUp() 
    {
        // stop the service
    }
    
    
    [Given("the ExternalDataRetrieverService is started")]
    public void GivenTheEDRServiceIsStarted()
    {
        _scenarioContext.Pending();
        // how to start and kill process for tests
        
        ProcessStartInfo startInfo = new ProcessStartInfo()
        {
            FileName = "foo/bar.sh",
            Arguments = "arg1 arg2 arg3",
        };
        Process proc = new Process()
        {
            StartInfo = startInfo,
        };
        proc.Start();
    }
    
    [When("I post a Get request")]
    public void WhenTheTwoNumbersAreAdded()
    {
        //TODO: implement act (action) logic

        _scenarioContext.Pending();
    }

    [Then("the result should be (.*)")]
    public void ThenTheResultShouldBe(int result)
    {
        //TODO: implement assert (verification) logic

        _scenarioContext.Pending();
    }
}
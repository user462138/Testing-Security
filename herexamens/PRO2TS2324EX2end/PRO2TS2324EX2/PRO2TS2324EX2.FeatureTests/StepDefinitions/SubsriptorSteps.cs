using Xunit.Gherkin.Quick;
using PRO2TS2324EX2;
using Moq;


namespace PRO2TS2324EX2.FeatureTests.StepDefinitions;

[FeatureFile("./Features/Subscriptor.Feature")]
public sealed class SubscriptorSteps : Feature
{
    private Subscriptor subscriptor;
    private Mock<IMailService> mockMailService;
    private Mock<IRegisterService> mockRegisterService;
    private Mock<IResultsService> mockResultService;

    public SubscriptorSteps() 
    {
        // Initialiseer de mocks
        mockMailService = new Mock<IMailService>();
        mockRegisterService = new Mock<IRegisterService>();
        mockResultService = new Mock<IResultsService>();
    }
    [Given(@"the subscriber is initialised")]
    public void InitialiseSubscriber()
    {
    }

    [When(@"both the results are not sufficient")]
    public void GetInsufficientResults()
    {
    }

    [Then(@"a mail is sent to the student")]
    public void CheckMailSent()
    {
    }
}

namespace PRO2TS2324EX2.IntegrationTests
{
    public class Tests
    {
        ResultsService resultService = new ResultsService();
        MailService mailService = new MailService();
        RegisterService registerService = new RegisterService();
        Logger logger = new Logger();

        private const string UrlMockoon = "http://localhost:3000/results";
        private const string UrlMockoonException = "http://localhost:3000/results/exception";

        [SetUp]
        public void Setup()
        {
            resultService = new ResultsService();
            mailService = new MailService();
            registerService = new RegisterService();
            logger = new Logger();
        }

        [Test]
        public void MailStudentNogoWhenStudentFailsBothTests()
        {
            Assert.Pass();
        }
    }
}
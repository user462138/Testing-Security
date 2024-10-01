using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Moq;

namespace PRO2TS2324EX2.UnitTests
{
    [TestClass]
    public class Tests
    {
/*        private Mock<ResultsService> resultService = null;
        private Mock<MailService> mailService = null;
        private Mock<RegisterService> registerService = null;
        private Mock<Logger> logger = null;
        private Mock<Results> results = null;
        private Subscriptor subscriptor;
        private Mock<IMailService> mockMailService;
        private Mock<IRegisterService> mockRegisterService;
        private Mock<IResultsService> mockResultService;

        public void Initialize()
        {
            resultService = new Mock<ResultsService>();
            mailService = new Mock<MailService>();
            registerService = new Mock<RegisterService>();
            logger = new Mock<Logger>();
            results = new Mock<Results>();
            mockMailService = new Mock<IMailService>();
            mockRegisterService = new Mock<IRegisterService>();
            mockResultService = new Mock<IResultsService>();
        }
*/
        [TestMethod]
        public void MailStudentNogoWhenStudentFailsBothTests()
        {
            // Arrange
            var resultServiceMock = new Mock<ResultsService>();
            var mailServiceMock = new Mock<MailService>();
            var registerServiceMock = new Mock<RegisterService>();
            var loggerMock = new Mock<Logger>();

            resultServiceMock.Setup(r => r.GetResults("janvandenpoel"))
                             .Returns(new Results { logisch_denken = 4, werkethiek = 4 });

            var subscriptor = new Subscriptor(resultServiceMock.Object, mailServiceMock.Object, registerServiceMock.Object, loggerMock.Object);

            // Act
            subscriptor.Evaluate();

            // Assert
            mailServiceMock.Verify(m => m.SendMail("janvandenpoel", "Je kan beter een andere opleiding kiezen"), Times.Once);
        }
    }
}
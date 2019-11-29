using EmailTests.TestData;
using Models;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PageObjects;
using NLog;

namespace EmailTests
{
    [TestFixture]
    public class EmailTests
    {
        private IWebDriver _driver;

        private ILogger _logger;

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            _logger = LogManager.GetLogger($"{TestContext.CurrentContext.Test.Name}");
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
        }

        [Test]
        public void SendEmail()
        {
            LogIn();
            //Assert.That(_driver.Url.Contains("/inbox/"));

            var emailForSending = new Email() { Recipient = Users.User.Login, Subject = "for fun", Text = "hey,there!Hru?" };

            var email = CreateEmail(emailForSending);

            email.SaveDraft();

            var mailPage = new MailPage(_driver, _logger);

            mailPage.SendDraft(emailForSending);

            Assert.That(mailPage.LetterIsInDrafts(emailForSending), Is.False);

            Assert.That(mailPage.LetterIsSent(emailForSending), Is.True);
        }

        private void LogIn()
        {
            new MailRuPage(_driver, _logger).Open().LogIn(Users.User);
        }

        private NewEmail CreateEmail(Email email)
        {
            return new NewEmail(_driver, _logger).Create(email);
        }
    }
}

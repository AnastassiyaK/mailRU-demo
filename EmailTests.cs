using Core.Driver;
using Core.Enums;
using EmailTests.TestData;
using Models;
using NLog;
using NUnit.Framework;
using PageObjects;
using System;

namespace EmailTests
{
    [TestFixture]
    public class EmailTests
    {
        private WebDriver _driver;

        private ILogger _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = LogManager.GetLogger($"{TestContext.CurrentContext.Test.Name}");
            _driver = new WebDriver(_logger);
            _driver.InitiateBrowser(BrowserType.Chrome);
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

            var emailForSending = new Email()
            {
                Recipient = Users.User.Login,
                Subject = "for fun",
                Text = $"Happy {DateTime.Today.DayOfWeek} {DateTime.Now.ToString()}"
            };

            var email = CreateEmail(emailForSending);

            email.SaveDraft();

            var mailPage = new MailPage(_driver, _logger);

            mailPage.SendDraft(emailForSending);

            Assert.That(mailPage.LetterIsInDrafts(emailForSending), Is.False, "Message is still in Drafts Folder");

            Assert.That(mailPage.LetterIsSent(emailForSending), Is.True, "Message is not in Sent Folder");

            mailPage.LogOut();
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

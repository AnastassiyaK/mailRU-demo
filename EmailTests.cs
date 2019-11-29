using EmailTests.TestData;
using Models;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PageObjects;

namespace EmailTests
{
    [TestFixture]
    public class EmailTests
    {
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
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

            var emailForSending = new Email() { Recipient = Users.User.Login, Subject = "for fun", Text = "hey,there!" };

            var email = CreateEmail(emailForSending);

            email.SaveDraft();


        }

        private void LogIn()
        {
            new MailRuPage(_driver).Open().LogIn(Users.User);
        }

        private NewEmail CreateEmail(Email email)
        {
            return new NewEmail(_driver).Create(email);
        }

        private void GetLastDraft()
        {

        }
    }
}

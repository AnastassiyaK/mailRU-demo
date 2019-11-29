using Models;
using OpenQA.Selenium;

namespace PageObjects
{
    public class MailRuPage : BasePageObject
    {
        private static string url = "https://mail.ru/";

        public MailRuPage(IWebDriver driver)
            : base(driver)
        {
        }

        private MailBox MailBox
        {
            get
            {
                return new MailBox(_driver);
            }
        }

        public void LogIn(User user)
        {
            MailBox.LogIn(user);
        }

        public MailRuPage Open()
        {
            _driver.Navigate().GoToUrl(url);
            return this;
        }        
    }
}

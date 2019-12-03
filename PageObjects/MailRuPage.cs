using Core.Driver;
using Models;
using NLog;

namespace PageObjects
{
    public class MailRuPage : BasePageObject
    {
        private static readonly string url = "https://mail.ru/";

        public MailRuPage(WebDriver driver, ILogger logger)
            : base(driver, logger)
        {
        }

        private MailBox MailBox
        {
            get
            {
                return new MailBox(_driver, _logger);
            }
        }

        public void LogIn(User user)
        {
            MailBox.LogIn(user);
        }

        public MailRuPage Open()
        {
            _driver.Navigate(url);
            return this;
        }
    }
}

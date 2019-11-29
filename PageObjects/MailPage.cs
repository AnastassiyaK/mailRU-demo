using Models;
using OpenQA.Selenium;

namespace PageObjects
{
    public class MailPage : BasePageObject
    {
        public MailPage(IWebDriver driver)
            : base(driver)
        {
        }

        private NewEmail NewEmail
        {
            get
            {
                return new NewEmail(_driver);
            }
        }

        public void SendMailTo(User user)
        {

        }

        public void CreateDraftMailTo(User user)
        {

        }
    }
}

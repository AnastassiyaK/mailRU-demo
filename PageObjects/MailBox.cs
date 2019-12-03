using Core.Driver;
using Models;
using NLog;
using OpenQA.Selenium;

namespace PageObjects
{
    internal class MailBox : BasePageObject
    {
        public MailBox(WebDriver driver, ILogger logger)
            : base(driver, logger)
        {
        }

        private IWebElement Login
        {
            get
            {
                return _driver.FindElement(By.CssSelector("input[id='mailbox:login']"));
            }
        }

        private IWebElement Password
        {
            get
            {
                return _driver.FindElement(By.CssSelector("input[id='mailbox:password']"));
            }
        }

        private IWebElement Submit
        {
            get
            {
                return _driver.FindElement(By.CssSelector("input[class='o-control']"));
            }
        }

        public void LogIn(User user)
        {
            Login.SendKeys(user.Login);
            Submit.Click();

            _driver.WaitForElementDisplayed(By.CssSelector("input[id='mailbox:password']"));
            
            Password.SendKeys(user.Password);
            Submit.Click();
        }
    }
}

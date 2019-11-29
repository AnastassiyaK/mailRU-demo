using Models;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace PageObjects
{
    internal class MailBox : BasePageObject
    {
        public MailBox(IWebDriver driver, ILogger logger)
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
            var waitor = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            waitor.Until(driver => Password.Displayed);
            Password.SendKeys(user.Password);
            Submit.Click();
        }
    }
}

using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace PageObjects
{
    public abstract class BasePageObject
    {
        protected readonly IWebDriver _driver;

        protected readonly ILogger _logger;

        public BasePageObject(IWebDriver driver, ILogger logger)
        {
            _driver = driver;
            _logger = logger;
        }

        protected virtual void WaitForPageLoad()
        {
            var waitDOM = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            IJavaScriptExecutor js = _driver as IJavaScriptExecutor;

            waitDOM.Until(driver => (bool)js.ExecuteScript("return document.readyState == 'complete'"));
        }
    }
}

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace PageObjects
{
    public class BasePageObject
    {
        protected readonly IWebDriver _driver;

        public BasePageObject(IWebDriver driver)
        {
            _driver = driver;
        }

        protected virtual void WaitForPageLoad()
        {
            var waitDOM = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            IJavaScriptExecutor js = _driver as IJavaScriptExecutor;

            waitDOM.Until(driver => (bool)js.ExecuteScript("return document.readyState == 'complete'"));
        }
    }
}

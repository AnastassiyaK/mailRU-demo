using Core.Enums;
using Core.Exceptions;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;

namespace Core.Driver
{
    public class WebDriver
    {
        private IWebDriver _driver;

        private ILogger _logger;

        public WebDriver(ILogger logger)
        {
            _logger = logger;
        }

        public void InitiateBrowser(BrowserType type)
        {
            switch (type)
            {
                case BrowserType.Chrome:
                    _driver = new ChromeDriver();
                    break;
                case BrowserType.Firefox:
                    _driver = new FirefoxDriver();
                    break;
                case BrowserType.InternetExplorer:
                    _driver = new InternetExplorerDriver();
                    break;
                default:
                    throw new ErrorDriverInitializationException(type.ToString());
            }

            _driver.Manage().Window.Maximize();
        }

        public void WaitForReadyDOM()
        {
            var waitDOM = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            IJavaScriptExecutor js = _driver as IJavaScriptExecutor;

            waitDOM.Until(driver => (bool)js.ExecuteScript("return document.readyState == 'complete'"));
        }

        public IWebElement FindElement(By locator)
        {
            return TryToFindElement(locator);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By locator)
        {
            return TryToFindElements(locator);
        }

        public ReadOnlyCollection<IWebElement> FindElements(IWebElement element, By locator)
        {
            return TryToFindElements(element, locator);
        }

        public void Navigate(string url)
        {
            _driver.Navigate().GoToUrl(url);
            _logger.Debug($"Driver navigated to {url}");
        }

        public void WaitForElementDisplayed(By locator)
        {
            var waitor = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            waitor.Until(driver =>
            {
                try
                {
                    return driver.FindElement(locator).Displayed;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }

            });
        }

        public void WaitForElementDisappear(By locator)
        {
            var waitor = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            waitor.Until(driver =>
            {
                try
                {
                    driver.FindElement(locator).Click();
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
                catch (ElementNotInteractableException)
                {
                    return true;
                }

            });
        }

        public void WaitForStyleProperties(IWebElement element, string cssAttribute, string[] cssValues)
        {
            var waitor = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            waitor.Until(driver =>
            {
                var loaderStyle = element.GetAttribute(cssAttribute);
                var containsValue = false;
                foreach (var value in cssValues)
                {
                    containsValue = loaderStyle.Contains(value);
                }
                return containsValue;
            });
        }

        public void ExecuteScriptOnElement(string script, IWebElement element)
        {
            script = script ?? throw new ArgumentNullException(nameof(script));

            IJavaScriptExecutor js = _driver as IJavaScriptExecutor;
            _logger.Trace($"Executing script {script} on element");
            js.ExecuteScript(script, element);
        }

        public void MoveToElement(IWebElement element)
        {
            Actions action = new Actions(_driver);
            action.MoveToElement(element).Perform();
            _logger.Debug($"Driver moved to element {element.Text}");
        }

        public void Quit()
        {
            _driver.Quit();
        }

        private IWebElement TryToFindElement(By locator)
        {
            IWebElement element = null;
            _logger.Debug($"Driver is performing search on the page by locator {locator}.");
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(driver =>
            {
                try
                {
                    element = driver.FindElement(locator);
                    return true;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            _logger.Debug($"Requested element was displayed.");
            return element;
        }

        private ReadOnlyCollection<IWebElement> TryToFindElements(By locator)
        {
            ReadOnlyCollection<IWebElement> elements = null;
            _logger.Debug($"Driver is performing search on the page by locator {locator}.");
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(driver =>
            {
                try
                {
                    elements = driver.FindElements(locator);
                    return true;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            _logger.Debug($"Requested elements were displayed.");
            return elements;
        }

        private ReadOnlyCollection<IWebElement> TryToFindElements(IWebElement element, By locator)
        {
            ReadOnlyCollection<IWebElement> elements = null;
            _logger.Debug($"Driver is performing search on the page by locator {locator}.");
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(driver =>
            {
                try
                {
                    elements = element.FindElements(locator);
                    return true;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });

            _logger.Debug($"Requested elements were displayed.");
            return elements;
        }
    }
}

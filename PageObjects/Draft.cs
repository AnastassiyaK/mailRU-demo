using NLog;
using OpenQA.Selenium;

namespace PageObjects
{
    public class Draft : Letter
    {
        public Draft(IWebElement element, IWebDriver driver, ILogger logger)
            : base(element, driver, logger)
        {
        }
    }
}

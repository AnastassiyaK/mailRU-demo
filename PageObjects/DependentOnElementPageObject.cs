using NLog;
using OpenQA.Selenium;

namespace PageObjects
{
    public abstract class DependentOnElementPageObject : BasePageObject
    {
        protected IWebElement _element;
        public DependentOnElementPageObject(IWebElement element, IWebDriver driver, ILogger logger)
            : base(driver, logger)
        {
            _element = element;
        }
    }
}

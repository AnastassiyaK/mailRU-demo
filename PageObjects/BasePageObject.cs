using Core.Driver;
using NLog;

namespace PageObjects
{
    public abstract class BasePageObject
    {
        protected readonly WebDriver _driver;

        protected readonly ILogger _logger;

        public BasePageObject(WebDriver driver, ILogger logger)
        {
            _driver = driver;
            _logger = logger;
        }

        protected virtual void WaitForPageLoad()
        {
            _driver.WaitForReadyDOM();
        }
    }
}

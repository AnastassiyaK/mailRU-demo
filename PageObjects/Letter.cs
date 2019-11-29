using NLog;
using OpenQA.Selenium;

namespace PageObjects
{
    public class Letter : DependentOnElementPageObject
    {
        public Letter(IWebElement element, IWebDriver driver, ILogger logger)
            : base(element, driver, logger)
        {
        }

        public string Recipient
        {
            get
            {
                return Addressee.Text;
            }
        }

        public string Theme
        {
            get
            {
                return Subject.Text;
            }
        }

        public string Text
        {
            get
            {
                return Body.Text;
            }
        }

        protected IWebElement Subject
        {
            get
            {
                return _element.FindElement(By.ClassName("ll-sj__normal"));
            }
        }

        protected IWebElement Addressee
        {
            get
            {
                return _element.FindElement(By.ClassName("ll-crpt"));
            }
        }

        protected IWebElement Body
        {
            get
            {
                return _element.FindElement(By.ClassName("ll-sp__normal"));
            }
        }

        public Letter Open()
        {
            Addressee.Click();

            _logger.Debug("Letter is opened.");

            return this;
        }

        public void Send()
        {
            new NewEmail(_driver, _logger).Send();
        }
    }
}

using Models;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace PageObjects
{
    public class NewEmail : BasePageObject
    {
        public NewEmail(IWebDriver driver, ILogger logger)
            : base(driver, logger)
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

        private IWebElement Addressee
        {
            get
            {
                _logger.Debug("Trying to get Addressee");
                return _driver.FindElement(By.CssSelector("div[data-type='to'] input"));
            }
        }

        private IWebElement Subject
        {
            get
            {
                _logger.Debug("Trying to get Subject");
                return _driver.FindElement(By.CssSelector("input[name='Subject']"));
            }
        }

        private IWebElement Body
        {
            get
            {
                _logger.Debug("Trying to get Body");
                return _driver.FindElement(By.CssSelector("div[role='textbox']"));
            }
        }

        private IWebElement NewMail
        {
            get
            {
                return _driver.FindElement(By.ClassName("compose-button__txt"));
            }
        }

        private IWebElement SendMail
        {
            get
            {
                _logger.Debug("Trying to press Sent Email button");
                return _driver.FindElement(By.CssSelector("span[data-title-shortcut='Ctrl+Enter']"));
            }
        }

        private IWebElement MakeDraft
        {
            get
            {
                _logger.Debug("Trying to save Draft Email");
                return _driver.FindElement(By.CssSelector("span[data-title-shortcut='Ctrl+S']"));
            }
        }

        private IWebElement CloseForm
        {
            get
            {
                _logger.Debug("Trying to close an email");
                return _driver.FindElement(By.CssSelector(".focus-zone button[title='Закрыть']"));
            }
        }

        private IWebElement WindowBlock
        {
            get
            {
                return _driver.FindElement(By.ClassName("layer-window__block"));
            }
        }

        public NewEmail Create(Email email)
        {
            OpenForm();

            Addressee.SendKeys(email.Recipient);
            Subject.SendKeys(email.Subject);
            Body.SendKeys(email.Text);

            _logger.Debug("Email was filled");
            return this;
        }

        public void SaveDraft()
        {
            WaitForPageLoad();

            MakeDraft.Click();
            _logger.Debug("Draft was saved");

            CloseForm.Click();
            _logger.Debug("Email was closed");
        }

        public void Send()
        {
            SendMail.Click();
            _logger.Debug("Email was sent");

            SkipAfterSentWindow();
        }

        private void OpenForm()
        {
            WaitForPageLoad();

            NewMail.Click();

            _logger.Debug("New Email window was opened");

            WaitForLoad();
        }

        private void WaitForLoad()
        {
            var waitor = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            waitor.Until(driver =>
            {
                try
                {
                    return Addressee.Displayed;
                }

                catch (NoSuchElementException)
                {
                    return false;
                }
            });
            _logger.Debug("New Email is successfully opened");
        }

        protected override void WaitForPageLoad()
        {
            var waitor = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            waitor.Until(driver =>
            {
                try
                {
                    var loaderStyle = _driver.FindElement(By.Id("app-loader")).GetAttribute("style");
                    return loaderStyle.Contains("display: none") && loaderStyle.Contains("opacity");
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
        }

        private void SkipAfterSentWindow()
        {
            WaitForPageLoad();
            WindowBlock.FindElement(By.ClassName("button2__ico")).Click();
        }
    }
}

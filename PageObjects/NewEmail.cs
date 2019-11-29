using Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace PageObjects
{
    public class NewEmail : BasePageObject
    {
        public NewEmail(IWebDriver driver)
            : base(driver)
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
                return _driver.FindElement(By.CssSelector("div[data-type='to'] input"));
            }
        }

        private IWebElement Subject
        {
            get
            {
                return _driver.FindElement(By.CssSelector("input[name='Subject']"));
            }
        }

        private IWebElement Body
        {
            get
            {
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

        private IWebElement Buttons
        {
            get
            {
                return _driver.FindElement(By.CssSelector(".compose-app__buttons"));
            }
        }
        private IWebElement SendMail
        {
            get
            {
                return Buttons.FindElement(By.CssSelector("span[data-title-shortcut='Ctrl+Enter']"));
            }
        }

        private IWebElement DraftMail
        {
            get
            {
                return Buttons.FindElement(By.CssSelector("span[data-title-shortcut='Ctrl+S']"));
            }
        }

        private IWebElement CloseMail
        {
            get
            {
                return _driver.FindElement(By.CssSelector(".focus-zone button[title='Закрыть']"));
            }
        }

        public NewEmail Create(Email email)
        {
            Open();

            Addressee.SendKeys(email.Recipient);
            Subject.SendKeys(email.Subject);
            Body.SendKeys(email.Text);

            return this;
        }

        public void SaveDraft()
        {
            DraftMail.Click();
            CloseMail.Click();
        }

        public void Send()
        {
            SendMail.Click();
        }

        private void Open()
        {
            WaitForPageLoad();

            NewMail.Click();

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
        }

        protected override void WaitForPageLoad()
        {
            var waitor = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            waitor.Until(driver =>
            {
                try
                {
                    return NewMail.Displayed;
                }

                catch (NoSuchElementException)
                {
                    return false;
                }
            });
        }
    }
}

using Models;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PageObjects
{
    public class MailPage : BasePageObject
    {
        public MailPage(IWebDriver driver, ILogger logger)
            : base(driver, logger)
        {
        }

        public List<Draft> Drafts
        {
            get
            {
                if (FolderIsNotOpened(FolderType.Draft))
                {
                    OpenDraftsFolder();
                }

                return _driver.FindElements(By.ClassName("llc__container"))
                    .Select(element => new Draft(element, _driver, _logger))
                    .ToList();
            }
        }

        public List<Letter> SentLetters
        {
            get
            {
                if (FolderIsNotOpened(FolderType.Sent))
                {
                    OpenSentFolder();
                }

                return _driver.FindElements(By.ClassName("llc__container"))
                    .Select(element => new Letter(element, _driver, _logger))
                    .ToList();
            }
        }

        private IWebElement DraftFolder
        {
            get
            {
                return _driver.FindElement(By.CssSelector("a[href='/drafts/']"));
            }
        }

        private IWebElement SentFolder
        {
            get
            {
                return _driver.FindElement(By.CssSelector("a[href='/sent/']"));
            }
        }

        public bool LetterIsSent(Email email)
        {
            string emailText = GetTextFromEmail(email);
            var found = SentLetters
                .Where(s => s.Recipient == email.Recipient && s.Theme == email.Subject && s.Text.Contains(emailText))
                .FirstOrDefault();

            return found == null ? false : true;
        }

        public bool LetterIsInDrafts(Email email)
        {
            string emailText = GetTextFromEmail(email);
            var found = Drafts
                .Where(s => s.Recipient == email.Recipient && s.Theme == email.Subject && s.Text.Contains(emailText))
                .FirstOrDefault();

            return found == null ? false : true;
        }

        public Letter OpenDraft(Email email)
        {
            string emailText = GetTextFromEmail(email);
            _logger.Debug("Opening created draft");

            return Drafts.Where(d => d.Recipient == email.Recipient && d.Theme == email.Subject && d.Text.Contains(emailText))
                .First()
                .Open();
        }

        public void SendDraft(Email email)
        {
            OpenDraft(email).Send();
        }

        private bool FolderIsNotOpened(FolderType type)
        {
            WaitForPageLoad();
            return !_driver.FindElements(By.CssSelector(".dataset__items a"))
                                .First()
                                .GetAttribute("href")
                                .Contains(type.ToString().ToLower());
        }

        private static string GetTextFromEmail(Email email)
        {
            var emailText = email.Text;
            if (email.Text.Length > 100)
            {
                emailText = email.Text.Substring(0, 100);
            }

            return emailText;
        }

        private void OpenDraftsFolder()
        {
            DraftFolder.Click();
        }

        private void OpenSentFolder()
        {
            var waitor = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            waitor.Until(driver =>
            {
                try
                {
                    SentFolder.Click();
                    return true;
                }

                catch (ElementClickInterceptedException)
                {
                    return false;
                }
            });
        }
    }
}

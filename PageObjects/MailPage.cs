using Core.Driver;
using Models;
using Models.Enums;
using Models.Exceptions;
using NLog;
using OpenQA.Selenium;
using PageObjects.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace PageObjects
{
    public class MailPage : BasePageObject
    {
        public MailPage(WebDriver driver, ILogger logger)
            : base(driver, logger)
        {
        }

        public List<Letter> Letters
        {
            get
            {
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

        private IWebElement Logout
        {
            get
            {
                return _driver.FindElement(By.CssSelector("a[id='PH_logoutLink']"));
            }
        }

        public bool LetterIsSent(Email email)
        {
            return LetterIsInFolder(email, FolderType.Sent);
        }

        public bool LetterIsInDrafts(Email email)
        {
            return LetterIsInFolder(email, FolderType.Drafts);
        }

        private bool LetterIsInFolder(Email email, FolderType type)
        {
            if (FolderIsNotOpened(type))
            {
                OpenFolder(type);
            }

            return GetLetterByTemplate(email) == null ? false : true;
        }

        public void SendDraft(Email email)
        {
            var draft = OpenDraft(email);

            if (draft != null)
            {
                draft.Send();
            }
            else
            {
                throw new DraftNotFoundException(email.Text);
            }
        }

        public void LogOut()
        {
            Logout.Click();
            WaitForLogout();
        }

        private Letter OpenDraft(Email email)
        {
            _logger.Debug("Opening created draft");

            if (FolderIsNotOpened(FolderType.Drafts))
            {
                OpenFolder(FolderType.Drafts);
            }

            return GetLetterByTemplate(email)?.Open();
        }

        private Letter GetLetterByTemplate(Email email)
        {
            string emailText = GetTextFromEmail(email);
            WaitForLoading();

            return Letters.Where(d => d.Recipient == email.Recipient && d.Theme == email.Subject && d.Text.Contains(emailText))
                            .FirstOrDefault();
        }

        private bool FolderIsNotOpened(FolderType type)
        {
            WaitForLoad();
            return !new FoldersNavigator(_driver, _logger).IsActive(type);
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

        private void OpenFolder(FolderType type)
        {
            WaitForLoad();
            switch (type)
            {
                case FolderType.Inbox:
                    break;
                case FolderType.Sent:
                    SentFolder.Click();
                    break;
                case FolderType.Drafts:
                    DraftFolder.Click();
                    break;
                case FolderType.Spam:
                    break;
                case FolderType.Trash:
                    break;
                default:
                    throw new InvalidFolderTypeException(type.ToString());
            }
            WaitForLoading();
        }

        private void WaitForLoad()
        {
            _driver.WaitForElementDisappear(By.CssSelector(".dimmer"));
        }

        private void WaitForLoading()
        {
            var progressBar = _driver.FindElement(By.ClassName("progress__value"));
            _driver.WaitForStyleProperties(progressBar, "style", new[] { "width: 100%;" });
        }

        private void WaitForLogout()
        {
            _driver.WaitForElementDisplayed(By.Id("auth-form"));
        }
    }
}

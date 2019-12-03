using Core.Driver;
using Models.Enums;
using NLog;
using OpenQA.Selenium;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PageObjects
{
    public class Folder : DependentOnElementPageObject
    {
        public Folder(IWebElement element, WebDriver driver, ILogger logger)
            : base(element, driver, logger)
        {
        }

        public FolderType Type
        {
            get
            {
                Regex regex = new Regex(@"\w+");
                var name = _element.GetAttribute("href").Replace("https://e.mail.ru", "");
                Match match = regex.Match(name);
                var type = match.Value.First().ToString().ToUpper() + match.Value.Substring(1);

                return (FolderType)Enum.Parse(typeof(FolderType), type);
            }
        }

        public bool IsActive
        {
            get
            {
                return _element.GetAttribute("class").Contains("active");
            }
        }
    }
}

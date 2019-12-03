using Core.Driver;
using Models.Enums;
using NLog;
using OpenQA.Selenium;
using PageObjects.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace PageObjects
{
    public class FoldersNavigator : BasePageObject
    {
        public FoldersNavigator(WebDriver driver, ILogger logger)
            : base(driver, logger)
        {
        }

        public List<Folder> Folders
        {
            get
            {
                return Navigator.FindElements(By.CssSelector(".nav__item"))
                .Select(element => new Folder(element, _driver, _logger))
                .ToList();
            }
        }

        private IWebElement Navigator
        {
            get
            {
                return _driver.FindElement(By.ClassName("nav-folders"));
            }
        }

        public bool IsActive(FolderType type)
        {
            var found = Folders.Where(f => f.Type == type).FirstOrDefault();
            return found == null ? throw new FolderNotFoundException(type.ToString()) : found.IsActive;
        }
    }
}

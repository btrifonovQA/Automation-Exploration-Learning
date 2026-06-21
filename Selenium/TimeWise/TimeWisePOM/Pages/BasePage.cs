using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;

namespace TimeWisePOM.Pages
{
    public class BasePage
    {
        protected readonly IWebDriver driver;
        protected readonly WebDriverWait wait;

        protected const string baseURL = "WEBSITE HAS BEEN DEPRECATED"; //site no longer up tests are for ref only
        protected const string email = "practise@domain.com";
        protected const string password = "swordfish123";

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        }

        protected void SendKeys(By by, string textToType)
        {
            try
            {
                var fieldToType = wait.Until(ExpectedConditions.ElementToBeClickable(by));
                fieldToType.Clear();
                fieldToType.SendKeys(textToType);
            }
            catch (Exception ex) when (ex is NoSuchElementException || ex is WebDriverTimeoutException)
            {
                throw new NoSuchElementException();
            }
        }

        protected void Click(By by)
        {
            try
            {
                var elementToClick = wait.Until(ExpectedConditions.ElementToBeClickable(by));
                elementToClick.Click();

            }
            catch (Exception ex) when (ex is NoSuchElementException || ex is WebDriverTimeoutException)
            {
                throw new NoSuchElementException();
            }
        }

        protected bool CheckElementIsVisible(By by)
        {
            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(by));
            }
            catch (Exception ex) when (ex is NoSuchElementException || ex is WebDriverTimeoutException)
            {
                return false;
            }
            return true;
        }

        protected IWebElement GetWebElement(By by)
        {
            try
            {
                var elementToGet = wait.Until(ExpectedConditions.ElementIsVisible(by));
                return elementToGet;
            }
            catch (Exception ex) when (ex is NoSuchElementException || ex is WebDriverTimeoutException)
            {
                throw new NoSuchElementException();
            }
        }

        protected ReadOnlyCollection<IWebElement> GetWebElements(By by)
        {
            try
            {
                var elementsToGet = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(by));
                return elementsToGet;
            }
            catch (Exception ex) when (ex is NoSuchElementException || ex is WebDriverTimeoutException)
            {
                throw new NoSuchElementException();
            }
        }

        protected string GetElementText(By by)
        {
            string elementText = string.Empty;

            try
            {
                var element = wait.Until(ExpectedConditions.ElementIsVisible(by));
                elementText = element.Text;
            }
            catch (Exception ex) when (ex is NoSuchElementException || ex is WebDriverTimeoutException)
            {
                throw new NoSuchElementException();
            }

            return elementText;
        }
    }
}

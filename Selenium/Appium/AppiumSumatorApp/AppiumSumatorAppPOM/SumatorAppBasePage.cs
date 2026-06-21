using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace AppiumSumatorAppPOM
{
    public class SumatorAppBasePage
    {
        private readonly AndroidDriver driver;
        private readonly WebDriverWait wait;

        private readonly By firstField = MobileBy.Id("com.example.androidappsummator:id/editText1");
        private readonly By secondField = MobileBy.Id("com.example.androidappsummator:id/editText2");
        private readonly By resultField = MobileBy.Id("com.example.androidappsummator:id/editTextSum");
        private readonly By buttonCalcSum = MobileBy.Id("com.example.androidappsummator:id/buttonCalcSum");

        public SumatorAppBasePage(AndroidDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        }

        protected IWebElement FindVisibleElement(By by)
        {
            return wait.Until(ExpectedConditions.ElementIsVisible(by));
        }

        protected IWebElement FindClickableElement(By by)
        {
            return wait.Until(ExpectedConditions.ElementToBeClickable(by));
        }

        protected void Click(By by)
        {
            var elementToClick = wait.Until(ExpectedConditions.ElementToBeClickable(by));
            elementToClick.Click();
        }

        protected void SendKeys(By by, string textToType)
        {
            var element = FindClickableElement(by);
            element.Clear();
            element.SendKeys(textToType);
        }

        public void Calculate(string num1, string num2)
        {
            SendKeys(firstField, num1);
            SendKeys(secondField, num2);

            Click(buttonCalcSum);
        }

        public string GetSumResult()
        {
            return FindVisibleElement(resultField).Text;
        }
    }
}
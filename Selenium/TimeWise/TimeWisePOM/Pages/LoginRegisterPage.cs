using OpenQA.Selenium;

namespace TimeWisePOM.Pages
{
    public class LoginRegisterPage : BasePage
    {
        public LoginRegisterPage(IWebDriver driver) : base(driver)
        {

        }

        private readonly By loginButton = By.XPath("//a[@id='tab-login']");
        private readonly By emailField = By.XPath("//input[@id='loginName']");
        private readonly By passwordField = By.XPath("//input[@id='loginPassword']");
        private readonly By signInButton = By.XPath("//button[@class='btn btn-primary btn-block mb-4']");

        protected const string Url = baseURL + "User/LoginRegister";

        public void Login()
        {
            Click(loginButton);
            SendKeys(emailField, email);
            SendKeys(passwordField, password);
            Click(signInButton);
        }

        public void OpenPage()
        {
            driver.Navigate().GoToUrl(Url);
        }
    }
}

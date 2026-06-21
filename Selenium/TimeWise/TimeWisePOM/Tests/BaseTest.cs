using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TimeWisePOM.Pages;

namespace TimeWisePOM.Tests
{
    public class BaseTest
    {
        public IWebDriver driver;
        public WebDriverWait wait;

        public static string lastCreatedCardName;

        public CreateTaskPage createTaskPage;
        public EditTaskPage editTaskPage;
        public HomePage homePage;
        public InProgressPage inProgressPage;
        public LoginRegisterPage loginRegisterPage;
        public ToDoPage toDoPage;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            chromeOptions.AddArgument("--disable-search-engine-choice-screen");
            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            chromeOptions.AddUserProfilePreference("profile.password_manager_leak_detection", false);
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            chromeOptions.AddArgument("--disable-save-password-bubble");

            driver = new ChromeDriver(chromeOptions);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            driver.Manage().Window.Maximize();

            createTaskPage = new CreateTaskPage(driver);
            editTaskPage = new EditTaskPage(driver);
            homePage = new HomePage(driver);
            inProgressPage = new InProgressPage(driver);
            loginRegisterPage = new LoginRegisterPage(driver);
            toDoPage = new ToDoPage(driver);

            // Log in to the application
            
            loginRegisterPage.OpenPage();
            loginRegisterPage.Login();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            driver.Quit();
            driver.Dispose();
        }

        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
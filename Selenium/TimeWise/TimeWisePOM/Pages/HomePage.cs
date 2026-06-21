using OpenQA.Selenium;

namespace TimeWisePOM.Pages
{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver driver) : base(driver)
        {

        }

        private readonly By toDoButton = By.XPath("//a[@href='/Task/ToDo']");
        private readonly By inProgressButton = By.XPath("//a[@href='/Task/InProgress']");
        private readonly By doneButton = By.XPath("//a[@href='/Task/Done']");

        protected const string Url = baseURL + "Home/Main";

        public void NavigateToDo()
        {
            Click(toDoButton);
        }

        public void NavigateInProgress()
        {
            Click(inProgressButton);
        }

        public void NavigateDone()
        {
            Click(doneButton);
        }
    }
}
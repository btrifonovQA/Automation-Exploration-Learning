using OpenQA.Selenium;

namespace TimeWisePOM.Pages
{
    public class InProgressPage : ToDoPage
    {
        public InProgressPage(IWebDriver driver) : base(driver)
        {

        }

        private readonly By deleteButton = By.XPath(".//a[text()='Delete']");
        private readonly By deleteYesConfirmationButton = By.XPath("//button[text()='Yes']");
        private readonly By toDoCardTaskNames = By.XPath(".//div[@class='card-body']/h5");

        public string Url = baseURL + "Task/InProgress";

        public void DeleteLastTask()
        {
            var lastTask = ReturnLastTask();
            lastTask.FindElement(deleteButton).Click();
            Click(deleteYesConfirmationButton);
        }

        public bool IsTaskNamePresent(string taskNameToCheck)
        {
            try
            {
                var cardTaskNames = GetWebElements(toDoCardTaskNames);

                foreach (var name in cardTaskNames)
                {
                    if (name.Text == taskNameToCheck)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
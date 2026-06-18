using OpenQA.Selenium;

namespace TimeWisePOM.Pages
{
    public class ToDoPage : BasePage
    {
        public ToDoPage(IWebDriver driver) : base(driver)
        {

        }

        private readonly By createTaskButton = By.XPath("//a[@href='/Task/Create']");
        private readonly By toDoCardsLocator = By.XPath("//section[@class='p-4 d-flex justify-content-center text-center w-100']/div");
        private readonly By toDoCardTaskNames = By.XPath(".//div[@class='card-body']/h5");
        private readonly By editButton = By.XPath(".//a[@class='btn btn-info'][text()='Edit']");
        private readonly By setStatusInProgressButton = By.XPath(".//a[text()='Set to \"In Progress\" status']");

        public string Url = baseURL + "Task/ToDo";

        public void ClickCreateNewTask()
        {
            Click(createTaskButton);
        }

        protected IWebElement ReturnLastTask()
        {
            var lastCard = GetWebElements(toDoCardsLocator).Last();
            return lastCard;
        }

        public void ClickEditLastTaskName()
        {
            var taskToEdit = ReturnLastTask();
            taskToEdit.FindElement(editButton).Click();
        }

        public string GetLastTaskName()
        {
            var lastTask = ReturnLastTask();
            var lastTaskName = lastTask.FindElement(toDoCardTaskNames).Text;

            return lastTaskName;
        }

        protected string GetTaskId()
        {
            var lastElement = ReturnLastTask();
            var cardId = lastElement.FindElement(editButton).GetAttribute("href");

            return cardId;
        }

        public void ChangeStatusInProgress()
        {
            var lastTask = ReturnLastTask();
            lastTask.FindElement(setStatusInProgressButton).Click();
        }
    }
}
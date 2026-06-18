using OpenQA.Selenium;

namespace TimeWisePOM.Pages
{
    public class EditTaskPage : ToDoPage
    {
        public EditTaskPage(IWebDriver driver) : base(driver)
        {

        }

        private readonly By taskNameField = By.XPath("//input[@id='form4Example1']");
        private readonly By editButton = By.XPath("//button[@class='btn btn-info btn-block mb-4 col-6']");

        public void EditTaskName(string editedTaskName)
        {
            SendKeys(taskNameField, editedTaskName);
            Click(editButton);
        }
    }
}

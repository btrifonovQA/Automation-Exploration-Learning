using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TimeWisePOM.Pages
{
    public class CreateTaskPage : BasePage
    {
        public CreateTaskPage(IWebDriver driver) : base(driver)
        {

        }

        private readonly By taskNameField = By.XPath("//input[@id='form4Example1']");
        private readonly By descriptionField = By.XPath("//textarea[@id='form4Example3']");
        private readonly By startDateField = By.XPath("//input[@id='datetimepicker1Input']");
        private readonly By endDateField = By.XPath("//input[@id='datetimepicker2Input']");
        private readonly By selectStatusField = By.XPath("//select[@id='form4Example3']");
        private readonly By createButton = By.XPath("//button[text()='Create']");

        public string Url = baseURL + "Task/Create";
        private const string date = "14/12/2025 10:00";

        public void FormFillBlank()
        {
            SendKeys(taskNameField, string.Empty);
            Click(createButton);
        }

        public void FillForm(string taskName, string description)
        {
            SendKeys(taskNameField, taskName);
            SendKeys(descriptionField, description);
            SendKeys(startDateField, date);
            SendKeys(endDateField, date);

            var select = new SelectElement(GetWebElement(selectStatusField));
            select.SelectByValue("10");

            Click(createButton);
        }

        public void ChangeTaskName(string editedTaskName)
        {
            SendKeys(taskNameField, editedTaskName);
        }

        public string GetURL()
        {
            return driver.Url;
        }
    }
}
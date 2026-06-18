using SeleniumExtras.WaitHelpers;

namespace TimeWisePOM.Tests
{
    [TestFixture]
    public class TimeWiseTests : BaseTest
    {

        private static string lastTaskName;

        private static string taskName;
        private static string description;
        private static string editedName;

        [Test, Order(1)]
        public void AddTaskWithoutNameTest_ShouldNotAllowCreation()
        {
            homePage.NavigateToDo();

            toDoPage.ClickCreateNewTask();

            createTaskPage.FormFillBlank();

            Assert.That(driver.Url, Is.EqualTo(createTaskPage.Url));
        }

        [Test, Order(2)]
        public void AddTaskWithRandomNameTest()
        {
            taskName = $"TaskID_{GenerateRandomString(5)}";
            description = $"This is the description for {taskName}";

            homePage.NavigateToDo();

            toDoPage.ClickCreateNewTask();

            createTaskPage.FillForm(taskName, description);

            wait.Until(ExpectedConditions.UrlToBe(toDoPage.Url));

            Assert.Multiple(() =>
            {
                Assert.That(driver.Url, Is.EqualTo(toDoPage.Url));
                Assert.That(toDoPage.GetLastTaskName(), Is.EqualTo(taskName));
            });
        }

        [Test, Order(3)]
        public void EditLastAddedTaskTest()
        {
            editedName = $"Edited_{taskName}";

            homePage.NavigateToDo();

            toDoPage.ClickEditLastTaskName();

            editTaskPage.EditTaskName(editedName);

            wait.Until(ExpectedConditions.UrlToBe(toDoPage.Url));

            Assert.Multiple(() =>
            {
                Assert.That(driver.Url, Is.EqualTo(toDoPage.Url));
                Assert.That(toDoPage.GetLastTaskName(), Is.EqualTo(editedName));
            });
        }

        [Test, Order(4)]
        public void MoveLastAddedTaskTest()
        {
            lastTaskName = toDoPage.GetLastTaskName();

            homePage.NavigateToDo();

            toDoPage.ChangeStatusInProgress();

            Assert.That(driver.PageSource, Does.Not.Contain(lastTaskName));
        }

        [Test, Order(5)]
        public void DeleteLastAddedTaskTest()
        {
            homePage.NavigateInProgress();

            inProgressPage.DeleteLastTask();

            wait.Until(ExpectedConditions.UrlToBe(inProgressPage.Url));

            Assert.Multiple(() =>
            {
                Assert.That(driver.Url, Is.EqualTo(inProgressPage.Url));
                Assert.That(inProgressPage.IsTaskNamePresent(lastTaskName), Is.False);
            });
        }
    }
}
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Service;

namespace ColorNoteAppPOM
{
    [TestFixture]
    public class NotesTests
    {
        private AndroidDriver driver;
        private AppiumLocalService appiumLocalService;
        private Notes notes;
        private string textTitle;
        private string editedTitle;

        [OneTimeSetUp]
        public void SetupBeforeAll()
        {
            appiumLocalService = new AppiumServiceBuilder()
                .WithIPAddress("127.0.0.1")
                .UsingPort(4723)
                .Build();

            appiumLocalService.Start();

            var appiumOptions = new AppiumOptions();
            appiumOptions.AutomationName = "UiAutomator2";
            appiumOptions.PlatformName = "Android";
            appiumOptions.PlatformVersion = "16";
            appiumOptions.DeviceName = "Pixel 9";
            appiumOptions.App = "C:\\Users\\(PC-NAME)\\Downloads\\Resources\\Notepad.apk"; //-> apk path
            appiumOptions.AddAdditionalAppiumOption("autoGrantPermissions", true);

            driver = new AndroidDriver(appiumLocalService.ServiceUrl, appiumOptions);

            notes = new Notes(driver);
            textTitle = "Test_1";
            editedTitle = "Edited_2";
        }

        [OneTimeTearDown]
        public void CleanupAfterAll()
        {
            driver?.Quit();
            driver?.Dispose();
            appiumLocalService.Dispose();
        }

        [Test, Order(1)]
        public void Test_CreateNote()
        {
            notes.CreateNote(textTitle, "text for test_1");

            Assert.That(notes.IsTitlePresentInNotesList(textTitle), Is.True, $"Notes with title {textTitle} was not present in notes list");
        }

        [Test, Order(2)]
        public void Test_EditNote()
        {
            notes.EditTitleOfNoteWithTitle(textTitle, editedTitle);

            Assert.That(notes.IsTitlePresentInNotesList(editedTitle), Is.True);
        }

        [Test, Order(3)]
        public void Test_DeleteNote()
        {
            notes.DeleteNoteWithTitle(editedTitle);

            Assert.That(notes.IsTitlePresentInNotesList(editedTitle), Is.False);
        }
    }
}
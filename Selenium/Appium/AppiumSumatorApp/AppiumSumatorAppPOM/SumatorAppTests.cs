using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Service;

namespace AppiumSumatorAppPOM
{
    [TestFixture]
    public class SumatorAppTests
    {
        private AndroidDriver driver;
        private AppiumLocalService appiumLocalService;
        private SumatorAppBasePage sumatorapp;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            appiumLocalService = new AppiumServiceBuilder().WithIPAddress("127.0.0.1").UsingPort(4723).Build();
            appiumLocalService.Start();

            var androidOptions = new AppiumOptions();
            androidOptions.AutomationName = "UiAutomator2";
            androidOptions.PlatformName = "Android";
            androidOptions.PlatformVersion = "16";
            androidOptions.DeviceName = "Pixel 9";
            androidOptions.App = "C:\\Users\\(PC-NAME)\\Downloads\\Resources\\com.example.androidappsummator.apk"; //or wherever app apk file path is

            driver = new AndroidDriver(appiumLocalService.ServiceUrl, androidOptions);

            sumatorapp = new SumatorAppBasePage(driver);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            driver?.Quit();
            driver?.Dispose();
            appiumLocalService.Dispose();
        }

        [Test]
        public void Test_ValidData()
        {
            sumatorapp.Calculate("40", "7");
            Assert.That(sumatorapp.GetSumResult(), Is.EqualTo("47"));
        }

        [Test]
        public void Test_InvalidData()
        {
            sumatorapp.Calculate(".", string.Empty);
            Assert.That(sumatorapp.GetSumResult(), Is.EqualTo("error"));
        }
    }
}
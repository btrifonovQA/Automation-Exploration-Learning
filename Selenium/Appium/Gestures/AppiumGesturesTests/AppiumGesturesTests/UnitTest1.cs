using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Interactions;
using System.Drawing;

namespace AppiumGesturesTests
{
    [TestFixture]
    public class AppiumGesturesTests
    {
        private AndroidDriver driver;
        private AppiumLocalService service;

        [OneTimeSetUp]
        public void SetupBeforeAll()
        {
            service = new AppiumServiceBuilder()
                .WithIPAddress("127.0.0.1")
                .UsingPort(4723)
                .Build();
            service.Start();

            var androidOptions = new AppiumOptions()
            {
                PlatformName = "Android",
                AutomationName = "UiAutomator2",
                App = "C:\\Users\\(PC-NAME)\\Downloads\\AppForTesting\\ApiDemos-debug.apk", //-> apk path
                DeviceName = "Pixel_9",
                PlatformVersion = "16.0",
            };

            driver = new AndroidDriver(service.ServiceUrl, androidOptions);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [OneTimeTearDown]
        public void CleanupAfterAll()
        {
            driver?.Quit();
            driver?.Dispose();
            service?.Dispose();
        }

        [Test]
        public void ScrollTest()
        {
            var views = driver.FindElement(MobileBy.XPath("//android.widget.TextView[@content-desc=\"Views\"]"));
            views.Click();

            ScrollToText("Lists");

            var lists = driver.FindElement(MobileBy.XPath("//android.widget.TextView[@content-desc=\"Lists\"]"));
            Assert.That(lists, Is.Not.Null, "Lists element was not found");

            lists.Click();

            var elementInLists = driver.FindElements(MobileBy.XPath("//android.widget.TextView[@content-desc=\"10. Single choice list\"]"));
            Assert.That(elementInLists, Is.Not.Null, "\"10. Single choice list\" element was not found");
        }

        [Test]
        public void SwipeTest()
        {
            var views = driver.FindElement(MobileBy.XPath("//android.widget.TextView[@content-desc=\"Views\"]"));
            views.Click();

            var gallery = driver.FindElement(MobileBy.XPath("//android.widget.TextView[@content-desc=\"Gallery\"]"));
            gallery.Click();

            var photos = driver.FindElement(MobileBy.XPath("//android.widget.TextView[@content-desc=\"1. Photos\"]"));
            photos.Click();

            var pic1 = driver.FindElement(By.XPath("//android.widget.Gallery[@resource-id=\"io.appium.android.apis:id/gallery\"]/android.widget.ImageView[1]"));

            var swipe = new Actions(driver);
            swipe.ClickAndHold(pic1)
                .MoveByOffset(-400, 0)
                .Release()
                .Build();
            swipe.Perform();

            var pic4 = driver.FindElement(By.XPath("//android.widget.Gallery[@resource-id=\"io.appium.android.apis:id/gallery\"]/android.widget.ImageView[4]"));

            Assert.That(pic4, Is.Not.Null, "Picture 4 not found");
        }

        [Test]
        public void DragAndDropTest()
        {
            var views = driver.FindElement(MobileBy.AccessibilityId("Views"));
            views.Click();

            var dragAndDrop = driver.FindElement(MobileBy.AccessibilityId("Drag and Drop"));
            dragAndDrop.Click();

            var dragPoint = driver.FindElement(By.XPath("//android.view.View[@resource-id=\"io.appium.android.apis:id/drag_dot_1\"]"));
            var dropPoint = driver.FindElement(By.XPath("//android.view.View[@resource-id=\"io.appium.android.apis:id/drag_dot_2\"]"));

            var scriptArgs = new Dictionary<string, object>()
            {
                {"elementId", dragPoint.Id },
                {"endX", dropPoint.Location.X + dropPoint.Size.Width / 2 },
                {"endY", dropPoint.Location.Y + dropPoint.Size.Height / 2  },
                {"speed", 2500 }
            };

            driver.ExecuteScript("mobile: dragGesture", scriptArgs);

            var dropMessage = driver.FindElement(MobileBy.Id("io.appium.android.apis:id/drag_result_text"));

            Assert.That(dropMessage.Text, Is.EqualTo("Dropped!"));
        }

        [Test]
        public void SlidingTest()
        {
            var views = driver.FindElement(MobileBy.AccessibilityId("Views"));
            views.Click();

            ScrollToText("Seek Bar");

            var seekbar = driver.FindElement(MobileBy.AccessibilityId("Seek Bar"));
            seekbar.Click();

            var scrollbar = driver.FindElement(MobileBy.Id("io.appium.android.apis:id/seek"));
            var startX = scrollbar.Location.X + scrollbar.Size.Width / 2;
            var startY = scrollbar.Location.Y + scrollbar.Size.Height / 2;
            var endX = startX * 2;
            var endY = startY;

            MoveSeekBarWithCordinates(startX, startY, endX, endY);

            Assert.That(scrollbar.Text, Is.EqualTo("100.0"));
        }

        [Test]
        public void ZoomInTest()
        {
            var views = driver.FindElement(MobileBy.AccessibilityId("Views"));
            views.Click();

            ScrollToText("WebView");

            var webview = driver.FindElement(MobileBy.AccessibilityId("WebView"));
            webview.Click();

            var titleElement = driver.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().text(\"This page is a Selenium sandbox\")"));
            Assert.That(titleElement, Is.Not.Null);

            Zoom(710, 1030, 938, 776, 570, 1202, 353, 1469);

            Assert.Throws<NoSuchElementException>(() =>
            {
                titleElement = driver.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().text(\"This page is a Selenium sandbox\")"));
            });
        }

        [Test]
        public void ZoomOutTest()
        {
            var views = driver.FindElement(MobileBy.AccessibilityId("Views"));
            views.Click();

            ScrollToText("WebView");

            var webview = driver.FindElement(MobileBy.AccessibilityId("WebView"));
            webview.Click();

            var titleElement = driver.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().text(\"This page is a Selenium sandbox\")"));
            Assert.That(titleElement, Is.Not.Null);

            Zoom(710, 1030, 938, 776, 570, 1202, 353, 1469); //Zoom in

            Assert.Throws<NoSuchElementException>(() =>
            {
                titleElement = driver.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().text(\"This page is a Selenium sandbox\")"));
            });

            Zoom(938, 776, 710, 1030, 353, 1469, 570, 1202); //Zoom out
            titleElement = driver.FindElement(MobileBy.AndroidUIAutomator("new UiSelector().text(\"This page is a Selenium sandbox\")"));
            Assert.That(titleElement, Is.Not.Null);
        }

        private void ScrollToText(string text)
        {
            driver.FindElement(MobileBy.AndroidUIAutomator(
                $"new UiScrollable(new UiSelector().scrollable(true)).scrollIntoView(new UiSelector().text(\"{text}\"))"));
        }

        private void MoveSeekBarWithCordinates(int startX, int startY, int endX, int endY)
        {
            var finger = new PointerInputDevice(PointerKind.Touch);

            var start = new Point(startX, startY);
            var end = new Point(endX, endY);

            var swipe = new ActionSequence(finger);
            swipe.AddAction(finger.CreatePointerMove(CoordinateOrigin.Viewport, start.X, start.Y, TimeSpan.Zero));
            swipe.AddAction(finger.CreatePointerDown(MouseButton.Left));
            swipe.AddAction(finger.CreatePointerMove(CoordinateOrigin.Viewport, end.X, end.Y, TimeSpan.FromSeconds(2)));
            swipe.AddAction(finger.CreatePointerUp(MouseButton.Left));

            driver.PerformActions(new List<ActionSequence> { swipe });
        }

        private void Zoom(int startX, int startY, int endX, int endY, int startX2, int startY2, int endX2, int endY2)
        {
            var finger1 = new PointerInputDevice(PointerKind.Touch);
            var finger2 = new PointerInputDevice(PointerKind.Touch);

            var zoomIn1 = new ActionSequence(finger1);
            zoomIn1.AddAction(finger1.CreatePointerMove(CoordinateOrigin.Viewport, startX, startY, TimeSpan.Zero));
            zoomIn1.AddAction(finger1.CreatePointerDown(MouseButton.Left));
            zoomIn1.AddAction(finger1.CreatePointerMove(CoordinateOrigin.Viewport, endX, endY, TimeSpan.FromMilliseconds(1500)));
            zoomIn1.AddAction(finger1.CreatePointerUp(MouseButton.Left));

            var zoomIn2 = new ActionSequence(finger2);
            zoomIn2.AddAction(finger2.CreatePointerMove(CoordinateOrigin.Viewport, startX2, startY2, TimeSpan.Zero));
            zoomIn2.AddAction(finger2.CreatePointerDown(MouseButton.Left));
            zoomIn2.AddAction(finger2.CreatePointerMove(CoordinateOrigin.Viewport, endX2, endY2, TimeSpan.FromMilliseconds(1500)));
            zoomIn2.AddAction(finger2.CreatePointerUp(MouseButton.Left));

            driver.PerformActions(new List<ActionSequence> { zoomIn1, zoomIn2 });
        }
    }
}

using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;

namespace ColorNoteAppPOM
{
    public class Notes
    {
        private readonly AndroidDriver driver;
        private readonly WebDriverWait wait;

        private readonly By skipTutorialButton = MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/btn_start_skip");
        private readonly By mainCreateNoteButton = MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/main_btn1");
        private readonly By createTypeTextButton = MobileBy.AndroidUIAutomator("new UiSelector().className(\"android.widget.LinearLayout\").instance(3)");
        private readonly By addNoteWhenEmptyButton = MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/empty_text");
        private readonly By notesListField = MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/note_list");
        private readonly By notesListFieldElements = MobileBy.XPath("//android.widget.TextView[@resource-id=\"com.socialnmobile.dictapps.notepad.color.note:id/title\"]");

        private readonly By editNoteTitleField = MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/edit_title");
        private readonly By editNoteTextField = MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/edit_note");
        private readonly By editNoteButton = MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/edit_btn");
        private readonly By editNoteBackButton = MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/back_btn");
        private readonly By editNoteMenuButton = MobileBy.Id("com.socialnmobile.dictapps.notepad.color.note:id/menu_btn");
        private readonly By editNoteDeleteButton = MobileBy.AndroidUIAutomator("new UiSelector().className(\"android.widget.LinearLayout\").instance(6)");
        private readonly By editNoteDeleteAlertOk = MobileBy.AndroidUIAutomator("new UiSelector().resourceId(\"android:id/button1\")");

        public Notes(AndroidDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        }

        private IWebElement FindElement(By by)
        {
            return wait.Until(ExpectedConditions.ElementIsVisible(by));
        }

        private ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            try
            {
                return wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
            }
            catch (WebDriverTimeoutException)
            {

                throw new NoSuchElementException();
            }
        }

        private void SendKeys(By by, string textToSend)
        {
            var element = FindElement(by);
            element.Clear();
            element.SendKeys(textToSend);
        }

        private void Click(By by)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(by)).Click();
        }

        public void SkipTutorial()
        {
            try
            {
                Click(skipTutorialButton);
            }
            catch (NoSuchElementException)
            {
                //if no tutorial button found continue.
            }
        }

        public void OpenCreateNoteMenu()
        {
            Click(mainCreateNoteButton);
        }

        public void OpenCreateNoteMenuWhenEmpty()
        {
            Click(addNoteWhenEmptyButton);
        }

        public void CreateTextNote()
        {
            Click(createTypeTextButton);
        }

        public void FillTextNoteTitle(string titleToCreateNoteWith)
        {
            SendKeys(editNoteTitleField, titleToCreateNoteWith);
        }

        public void FillTextNoteText(string textToCreateNoteWith)
        {
            SendKeys(editNoteTextField, textToCreateNoteWith);
        }

        public void FillCreatedNote(string titleToCreateNoteWith, string textToCreateNoteWith)
        {
            FillTextNoteTitle(titleToCreateNoteWith);
            FillTextNoteText(textToCreateNoteWith);
        }

        private void ConfirmNoteText()
        {
            Click(editNoteBackButton);
        }

        public void ConfirmNoteTextAndSave()
        {
            ConfirmNoteText();
            Click(editNoteBackButton);
        }

        public int GetNotesListCount()
        {
            return FindElements(notesListFieldElements).Count;
        }

        public bool IsTitlePresentInNotesList(string title)
        {
            try
            {
                if (GetNotesListCount() <= 0)
                {
                    throw new NoSuchElementException();
                }

                var notesList = FindElements(notesListFieldElements);
                return notesList.Any(note => note.Text == title);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public IWebElement GetNoteWithTitle(string title)
        {
            var notesList = FindElement(notesListField);
            int count = GetNotesListCount();
            bool isIt = IsTitlePresentInNotesList(title);

            if (GetNotesListCount() <= 0 || IsTitlePresentInNotesList(title) is false)
            {
                throw new NoSuchElementException();
            }

            var noteToEdit = FindElements(notesListFieldElements).First(element => element.Text == title);
            return noteToEdit;
        }

        public void EditTextOfNoteWithTitle(string title, string newText)
        {
            var titleToEdit = GetNoteWithTitle(title);
            wait.Until(ExpectedConditions.ElementToBeClickable(titleToEdit)).Click();

            Click(editNoteButton);
            SendKeys(editNoteTextField, newText);
            ConfirmNoteTextAndSave();
        }

        public void EditTitleOfNoteWithTitle(string title, string newTitle)
        {
            var titleToEdit = GetNoteWithTitle(title);
            wait.Until(ExpectedConditions.ElementToBeClickable(titleToEdit)).Click();

            Click(editNoteButton);
            SendKeys(editNoteTitleField, newTitle);
            ConfirmNoteTextAndSave();
        }

        public void DeleteNoteWithTitle(string title)
        {
            var noteToDelete = GetNoteWithTitle(title);
            wait.Until(ExpectedConditions.ElementToBeClickable(noteToDelete)).Click();

            Click(editNoteMenuButton);
            Click(editNoteDeleteButton);
            Click(editNoteDeleteAlertOk);
        }

        public void CreateNote(string title, string text)
        {
            SkipTutorial();
            OpenCreateNoteMenu();
            CreateTextNote();
            FillCreatedNote(title, text);
            ConfirmNoteTextAndSave();
        }
    }
}
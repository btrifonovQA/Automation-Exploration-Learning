namespace ItemManagementTests.Services
{
    [TestFixture]
    public class ValidateItemNameTests : ItemServiceTestBase
    {

        [TestCase("Valid Name")]
        [TestCase("1234567890")]
        public void ValidateItemName_ShouldReturnTrueIfItemNameIsValid(string name)
        {
            bool result = _itemService.ValidateItemName(name);

            Assert.That(result, Is.True);
        }

        [TestCase("Invalid Name")]
        [TestCase("")]
        [TestCase(null)]
        [TestCase(" ")]
        public void ValidateItemName_ShouldReturnFalseIfItemNameIsInvalid(string? name)
        {
            bool result = _itemService.ValidateItemName(name);

            Assert.That(result, Is.False, "Fails on whitespace *intended");
        }
    }
}
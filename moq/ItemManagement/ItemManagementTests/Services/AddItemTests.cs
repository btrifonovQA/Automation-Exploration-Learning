using ItemManagementLib.Models;
using Moq;

namespace ItemManagementTests.Services
{
    [TestFixture]
    public class AddItemTests : ItemServiceTestBase
    {

        [Test]
        public void AddItem_ShouldCallAddItemOnRepository()
        {
            _itemService.AddItem("Test Item");

            _mockRepository.Verify(repo => repo.AddItem(It.Is<Item>(item => item.Name == "Test Item")), Times.Once);
        }
    }
}
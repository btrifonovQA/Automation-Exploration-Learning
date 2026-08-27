using ItemManagementLib.Models;
using Moq;

namespace ItemManagementTests.Services
{
    [TestFixture]
    public class GetItemTests : ItemServiceTestBase
    {

        [Test]
        public void GetItem_ShouldCallGetItemOnRepository()
        {
            Item testItem = new Item { Id = 1, Name = "Item 1" };
            _mockRepository.Setup(repo => repo.GetItemById(1)).Returns(testItem);

            _itemService.GetItemById(1);

            _mockRepository.Verify(repo => repo.GetItemById(1), Times.Once);
        }

        [Test]
        public void GetItem_ShouldReturnItemFromRepository()
        {
            Item testItem = new Item { Id = 1, Name = "Item 1" };
            _mockRepository.Setup(repo => repo.GetItemById(1)).Returns(testItem);

            Item returnedItem = _itemService.GetItemById(1);

            Assert.That(returnedItem, Is.EqualTo(testItem));
        }

        [Test]
        public void GetAllItems_ShouldReturnAllItems()
        {
            List<Item> testItems = new List<Item>
            {
                new() { Id = 1, Name = "Item 1" },
                new() { Id = 2, Name = "Item 2" }
            };

            _mockRepository.Setup(repo => repo.GetAllItems()).Returns(testItems);

            IEnumerable<Item> actualItems = _itemService.GetAllItems();

            Assert.That(actualItems, Is.EquivalentTo(testItems));
        }
    }
}
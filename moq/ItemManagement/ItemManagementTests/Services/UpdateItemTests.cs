using ItemManagementLib.Models;
using Moq;

namespace ItemManagementTests.Services
{
    [TestFixture]
    public class UpdateItemTests : ItemServiceTestBase
    {

        [Test]
        public void UpdateItem_ShouldCallUpdateItemOnRepository()
        {
            Item testItem = new Item { Id = 1, Name = "Item 1" };
            _mockRepository.Setup(repo => repo.GetItemById(1)).Returns(testItem);


            _itemService.UpdateItem(1, "Updated Name");

            _mockRepository.Verify(repo => repo.UpdateItem(It.Is<Item>(item => item.Id == 1 && item.Name == "Updated Name")), Times.Once);
        }

        [Test]
        public void UpdateItem_ShouldNotUpdateItemWhenItemDoesNotExist()
        {
            _mockRepository.Setup(repo => repo.GetItemById(1)).Returns((Item?)null);

            _itemService.UpdateItem(1, "Test");

            _mockRepository.Verify(repo => repo.UpdateItem(It.IsAny<Item>()), Times.Never);
        }
    }
}
using Moq;

namespace ItemManagementTests.Services
{
    [TestFixture]
    public class DeleteItemTests : ItemServiceTestBase
    {

        [Test]
        public void DeleteItem_ShouldCallDeleteItemOnRepository()
        {
            _itemService.DeleteItem(1);

            _mockRepository.Verify(repo => repo.DeleteItem(1), Times.Once);
        }
    }
}
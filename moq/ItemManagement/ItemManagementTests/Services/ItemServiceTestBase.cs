using ItemManagementApp.Services;
using ItemManagementLib.Repositories;
using Moq;

public abstract class ItemServiceTestBase
{
    protected Mock<IItemRepository> _mockRepository;
    protected ItemService _itemService;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IItemRepository>();
        _itemService = new ItemService(_mockRepository.Object);
    }
}

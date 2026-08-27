using Moq;
using NotificationApp.Entities;
using NotificationApp.Interfaces;
using NotificationApp.Services;

namespace NotificationAppTests
{
    public class NotificationAppTests
    {
        private Mock<IUserRepository> _mockUserRepo;
        private Mock<INotifier> _mockNotifier;
        private NotificationService _notificationService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockNotifier = new Mock<INotifier>();

            _notificationService = new NotificationService(_mockUserRepo.Object, _mockNotifier.Object);
        }

        [Test]
        public void NotifyUser_WithValidActiveUser_CallsSend()
        {
            User testUser = new() { Id = 1, Email = "mock@example.com", IsActive = true };
            _mockUserRepo.Setup(userRepo => userRepo.GetUserById(1)).Returns(testUser);

            _notificationService.NotifyUser(1, "Mock Message!");

            _mockNotifier.Verify(notifier => notifier.Send(testUser.Email, "Mock Message!"), Times.Once);
        }

        [Test]
        public void NotifyUser_WithInactiveUser_ThrowsInvalidOperationException()
        {
            User testUser = new() { Id = 1, Email = "mock@example.com", IsActive = false };
            _mockUserRepo.Setup(userRepo => userRepo.GetUserById(1)).Returns(testUser);

            Assert.Throws<InvalidOperationException>(() => _notificationService.NotifyUser(testUser.Id, "Inactive User"));
            _mockNotifier.VerifyNoOtherCalls();
        }

        [Test]
        public void NotifyUser_WithNonExistentUser_ThrowsArgumentException()
        {
            // Simulate a database containing user 1, while requesting user 2.
            User testUser = new() { Id = 1, Email = "mock@example.com", IsActive = true };
            _mockUserRepo.Setup(userRepo => userRepo.GetUserById(1)).Returns(testUser);

            Assert.Throws<ArgumentException>(() => _notificationService.NotifyUser(2, "No user here!"));
            _mockNotifier.VerifyNoOtherCalls();
        }


        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void NotifyUser_WithNoMessage_ThrowsArgumentException(string? message)
        {

            Assert.Throws<ArgumentException>(() => _notificationService.NotifyUser(1, message));
            _mockUserRepo.Verify(userRepo => userRepo.GetUserById(It.IsAny<int>()), Times.Never);
            _mockNotifier.VerifyNoOtherCalls();
        }
    }
}
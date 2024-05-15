using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
using PearsonXApp.UseCases.Follow;
using PearsonXApp.UseCases.Users;

namespace PearsonXApp.UseCases.Tests
{
    public class FollowServiceTests
    {
        private readonly List<Entities.User> _existingUsers;
        public FollowServiceTests()
        {
            _existingUsers =
            [
                new Entities.User { Name = "@Bob" },
                new Entities.User { Name = "@Hank" },
                new Entities.User { Name = "@Claudia" },
            ];
        }


        [Fact]
        public void FollowUser_UserExists_FollowUserCalled()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();

            var followParams = new FollowDto { User = "@Bob", UserToFollow = "@Hank" };

            var followService = new FollowService(userServiceMock.Object);

            // Act
            followService.FollowUser(followParams, _existingUsers);

            // Assert
            userServiceMock.Verify(x => x.FollowUser(followParams), Times.Once);
        }

        [Fact]
        public void FollowUser_UserDoesNotExist_PrintErrorMessage()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            
            var followParams = new FollowDto { User="@Bob",UserToFollow = "@Sergio" };

            var followService = new FollowService(userServiceMock.Object);

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act
            followService.FollowUser(followParams, _existingUsers);

            // Assert
            userServiceMock.Verify(x => x.FollowUser(followParams), Times.Never);
            Assert.Equal("Cannot follow user @Sergio. User not found\r\n", stringWriter.ToString());
        }
    }
}

using Moq;
using PearsonXApp.UseCases.Post;
using PearsonXApp.UseCases.Users;

namespace PearsonXApp.UseCases.Tests
{
    public class PostServiceTests
    {
        [Fact]
        public void PostMessage_ShouldCallUserServicePostMessage()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var postService = new PostService(userServiceMock.Object);
            var postDto = new PostDto() { UserName = "@Bob" };

            // Act
            postService.PostMessage(postDto);

            // Assert
            userServiceMock.Verify(u => u.PostMessage(postDto), Times.Once);
        }

    }
}

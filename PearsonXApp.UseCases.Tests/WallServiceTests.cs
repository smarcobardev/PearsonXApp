using Moq;
using PearsonXApp.UseCases.Post;
using PearsonXApp.UseCases.Users;
using PearsonXApp.UseCases.Wall;

namespace PearsonXApp.UseCases.Tests
{
    public class WallServiceTests
    {
        [Fact]
        public void DisplayWall_ShouldCallDisplayWallMethodOfUserService()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var wallService = new WallService(userServiceMock.Object);
            var username = "@Bob";

            // Act
            wallService.DisplayWall(username);

            // Assert
            userServiceMock.Verify(u => u.DisplayWall(username), Times.Once());
        }

        [Fact]
        public void FormatMessage_ShouldReturnCorrectlyFormattedMessage()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var wallService = new WallService(userServiceMock.Object);

            var postDto = new PostDto
            {
                UserName = "@Bob",
                Message = "Hello, world!",
                DateTime = DateTime.UtcNow
            };

            var expectedMessage = $"{postDto.Message} {postDto.UserName} @{postDto.DateTime.ToLocalTime():HH:mm}";

            // Act
            var formattedMessage = wallService.FormatMessage(postDto);

            // Assert
            Assert.Equal(expectedMessage, formattedMessage);
        }

        [Fact]
        public void DisplayWall_ShouldWriteFormattedMessagesToConsole()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var wallService = new WallService(userServiceMock.Object);

            var username = "@Bob";

            var messages = new List<PostDto>
            {
                new() { UserName = "@Bob", Message = "Hello, world!", DateTime = DateTime.UtcNow },
                new() { UserName = "@Bob", Message = "How are you?", DateTime = DateTime.UtcNow.AddMinutes(5) }
            };

            userServiceMock.Setup(u => u.DisplayWall(username)).Returns(messages);

            var expectedFormattedMessages = new List<string>
            {
                $"{messages[0].Message} {messages[0].UserName} @{messages[0].DateTime.ToLocalTime():HH:mm}",
                $"{messages[1].Message} {messages[1].UserName} @{messages[1].DateTime.ToLocalTime():HH:mm}"
            };

            var consoleOutput = new List<string>();

            // Capture console output
            using (var consoleOutputWriter = new System.IO.StringWriter())
            {
                Console.SetOut(consoleOutputWriter);

                // Act
                wallService.DisplayWall(username);

                // Capture console output
                consoleOutput.AddRange(consoleOutputWriter.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries));
            }

            // Assert
            Assert.Equal(expectedFormattedMessages, consoleOutput);
        }
    }
}

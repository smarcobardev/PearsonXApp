using Moq;
using PearsonXApp.Entities;
using PearsonXApp.UseCases.Follow;
using PearsonXApp.UseCases.Post;
using PearsonXApp.UseCases.Users.Seeder;
using PearsonXApp.UseCases.Wall;

namespace PearsonXApp.UseCases.Tests
{

    public class CommandExecutorTests
    {
        private readonly Mock<IPostService> _postServiceMock;
        private readonly Mock<IFollowService> _followServiceMock;
        private readonly Mock<IWallService> _wallServiceMock;
        private readonly Mock<IUserSeeder> _userSeederMock;
        private readonly CommandExecutor.CommandExecutor _commandExecutor;

        public CommandExecutorTests()
        {
            _postServiceMock = new Mock<IPostService>();
            _followServiceMock = new Mock<IFollowService>();
            _wallServiceMock = new Mock<IWallService>();
            _userSeederMock = new Mock<IUserSeeder>();

            _userSeederMock.Setup(u => u.SeedUsers()).Returns([new User() { Name = "@Bob" }, new User() { Name = "@Hank" }, new User() { Name = "@Claudia" }]);

            _commandExecutor = new CommandExecutor.CommandExecutor(_postServiceMock.Object, _followServiceMock.Object, _wallServiceMock.Object, _userSeederMock.Object);
        }

        [Fact]
        public void ExecuteCommand_InvalidCommand_PrintsErrorMessage()
        {
            // Arrange
            string[] commandParts = ["ReTweet"];
            

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act
            _commandExecutor.ExecuteCommand(commandParts);

            // Assert
            Assert.Contains("Invalid Command.", stringWriter.ToString());
        }

        [Fact]
        public void ExecuteCommand_UserNotFound_PrintsErrorMessage()
        {
            // Arrange
            string[] commandParts = ["post", "Sergio", "Hello"];

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act
            _commandExecutor.ExecuteCommand(commandParts);

            // Assert
            Assert.Contains("User @Sergio not found.", stringWriter.ToString());
        }

        [Fact]
        public void ExecuteCommand_ValidCommand_CallsPostService()
        {
            // Arrange
            string[] commandParts = ["post", "Bob", "Hello World"];

            // Act
            _commandExecutor.ExecuteCommand(commandParts);

            // Assert
            _postServiceMock.Verify(p => p.PostMessage(It.IsAny<PostDto>()), Times.Once);
        }

        [Fact]
        public void ExecuteCommand_ValidCommand_CallsFollowService()
        {
            // Arrange
            string[] commandParts = ["follow", "bob", "hank"];

            // Act
            _commandExecutor.ExecuteCommand(commandParts);

            // Assert
            _followServiceMock.Verify(f => f.FollowUser(It.IsAny<FollowDto>(), It.IsAny<List<User>>()), Times.Once);
        }

        [Fact]
        public void ExecuteCommand_ValidCommand_CallsWallService()
        {
            // Arrange
            string[] commandParts = ["wall", "bob"];

            // Act
            _commandExecutor.ExecuteCommand(commandParts);

            // Assert
            _wallServiceMock.Verify(w => w.DisplayWall(It.IsAny<string>()), Times.Once);
        }
    }
}
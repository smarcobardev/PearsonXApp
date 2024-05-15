using Microsoft.Extensions.DependencyInjection;
using Moq;
using PearsonXApp.UseCases.CommandExecutor;
using Xunit;

namespace PearsonXApp.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void StartConsoleApp_ShouldExecuteCommand_WhenValidCommandIsProvided()
        {
            // Arrange
            var serviceProviderMock = new Mock<IServiceProvider>();
            var commandExecutorMock = new Mock<ICommandExecutor>();

            serviceProviderMock.Setup(s => s.GetService(typeof(ICommandExecutor))).Returns(commandExecutorMock.Object);

            var input = "Post\nExit\n";
            var stringReader = new StringReader(input);
            Console.SetIn(stringReader);

            // Act
            Program.StartConsoleApp(serviceProviderMock.Object);

            // Assert
            commandExecutorMock.Verify(c => c.ExecuteCommand(It.IsAny<string[]>()), Times.Once());
        }

        [Fact]
        public void StartConsoleApp_ShouldNotExecuteCommand_WhenEmptyCommandIsProvided()
        {
            // Arrange
            var serviceProviderMock = new Mock<IServiceProvider>();
            var commandExecutorMock = new Mock<ICommandExecutor>();

            serviceProviderMock.Setup(s => s.GetService(typeof(ICommandExecutor))).Returns(commandExecutorMock.Object);

            // Act
            Program.StartConsoleApp(serviceProviderMock.Object);

            // Assert
            commandExecutorMock.Verify(c => c.ExecuteCommand(It.IsAny<string[]>()), Times.Never);
        }

        [Fact]
        public void ValidateExitCommand_ShouldReturnTrue_WhenExitCommandIsProvided()
        {
            // Arrange
            var commandParts = new string[] { "exit" };


            var input = "Exit";
            var stringReader = new StringReader(input);
            Console.SetIn(stringReader);

            // Act
            var result = Program.ExitCommandReceived(commandParts);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateExitCommand_ShouldReturnFalse_WhenNonExitCommandIsProvided()
        {
            // Arrange
            var commandParts = new string[] { "post" };

            // Act
            var result = Program.ExitCommandReceived(commandParts);

            // Assert
            Assert.False(result);
        }
    }
}

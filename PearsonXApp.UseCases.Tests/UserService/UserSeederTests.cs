using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using PearsonXApp.Entities;
using PearsonXApp.UseCases.Users.Seeder;

namespace PearsonXApp.UseCases.Tests.UserService
{
    public class UserSeederTests
    {
        [Fact]
        public void SeedUsers_ShouldReturnListOfUsers()
        {
            // Arrange
            var configMock = new Mock<IConfigurationRoot>();

            var users = new Dictionary<string, string> {
                { "Users:0:Name", "@Bob" },
                { "Users:1:Name", "@Hank" },
                { "Users:2:Name", "@Claudia" },
             };


            var configBuilder = new ConfigurationBuilder();

            var mockConfSections = configBuilder.AddInMemoryCollection(users)
                .Build()
                .GetSection("Users");

            configMock.Setup(x => x.GetSection("Users")).Returns(mockConfSections);

            var userSeeder = new UserSeeder(configMock.Object);

            // Act
            var result = userSeeder.SeedUsers();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<User>>(result);
            Assert.Equal(users.Count, result.Count);
        }
    }
}

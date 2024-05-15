using PearsonXApp.UseCases.Follow;
using PearsonXApp.UseCases.Post;

namespace PearsonXApp.UseCases.Tests.UserService
{
    public class UserServiceTests
    {

        private class TestUserService : UseCases.Users.UserService
        {
            public TestUserService() : base() { }

            public Dictionary<string, List<PostDto>> UserMessages => _userMessages;

            public Dictionary<string, List<string>> UserFollowers => _userFollowers;
        }

        [Fact]
        public void PostMessage_ShouldAddMessageToUserMessages()
        {
            // Arrange
            var userService = new TestUserService();

            var postDto = new PostDto
            {
                UserName = "@Bob",
                Message = "Hello, world!",
                DateTime = DateTime.UtcNow
            };

            // Act
            userService.PostMessage(postDto);

            // Assert
            Assert.True(userService.UserMessages.ContainsKey(postDto.UserName));
            Assert.Contains(postDto, userService.UserMessages[postDto.UserName]);
        }

        [Fact]
        public void FollowUser_ShouldAddUserToUserFollowers()
        {
            // Arrange
            var userService = new TestUserService();

            var followDto = new FollowDto
            {
                User = "@Bob",
                UserToFollow = "@Hank"
            };

            // Act
            userService.FollowUser(followDto);

            // Assert
            Assert.True(userService.UserFollowers.ContainsKey(followDto.User));
            Assert.Contains(followDto.UserToFollow, userService.UserFollowers[followDto.User]);
        }

        [Fact]
        public void FollowUser_ShouldNotAddDuplicateUserToUserFollowers()
        {
            // Arrange
            var userService = new TestUserService();

            var followDto = new FollowDto
            {
                User = "@Bob",
                UserToFollow = "@Hank"
            };

            // Act
            userService.FollowUser(followDto);
            userService.FollowUser(followDto);

            // Assert
            Assert.Single(userService.UserFollowers[followDto.User]);
        }

        [Fact]
        public void DisplayWall_ShouldReturnWallMessagesInDescendingOrder()
        {
            //Arrange
           var userService = new TestUserService();

            var user1 = "@Bob";
            var user2 = "@Hank";

            var postDto1 = new PostDto
            {
                UserName = user2,
                Message = "Hello, world!",
                DateTime = DateTime.UtcNow
            };

            var postDto2 = new PostDto
            {
                UserName = user2,
                Message = "Goodbye, world!",
                DateTime = DateTime.UtcNow.AddMinutes(10)
            };

            userService.PostMessage(postDto1);
            
            userService.PostMessage(postDto2);

            userService.FollowUser(new FollowDto { User = user1, UserToFollow = user2 });

            // Act
            var wallMessages = userService.DisplayWall(user1).ToList();

            // Assert
            Assert.Equal(2, wallMessages.Count);
            Assert.Equal(postDto2, wallMessages[0]);
            Assert.Equal(postDto1, wallMessages[1]);
        }
    }
}

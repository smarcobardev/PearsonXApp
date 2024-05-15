using PearsonXApp.UseCases.Follow;
using PearsonXApp.UseCases.Post;

namespace PearsonXApp.UseCases.Users
{
    public class UserService : IUserService
    {

        protected readonly Dictionary<string, List<PostDto>> _userMessages;
        protected readonly Dictionary<string, List<string>> _userFollowers;

        public UserService()
        {
            _userMessages = new Dictionary<string, List<PostDto>>(StringComparer.CurrentCultureIgnoreCase);
            _userFollowers = new Dictionary<string, List<string>>(StringComparer.CurrentCultureIgnoreCase);
        }

        public void PostMessage(PostDto postDto)
        {
            if (!_userMessages.TryGetValue(postDto.UserName, out List<PostDto>? value))
            {
                value ??= [];

                _userMessages[postDto.UserName] = value;
            }

            value.Add(postDto);

            Console.WriteLine($"{postDto.UserName} > posted \"{postDto.Message}\" @{postDto.DateTime.ToLocalTime():HH:mm}");

        }

        public void FollowUser(FollowDto followDto)
        {
            if (!_userFollowers.TryGetValue(followDto.User, out List<string>? value))
            {
                value ??= [];

                _userFollowers[followDto.User] = value;
            }

            if (value.Contains(followDto.UserToFollow))
            {
                Console.WriteLine($"{followDto.User} is already following {followDto.UserToFollow}");
                return;
            }

            value.Add(followDto.UserToFollow);

            Console.WriteLine($"{followDto.User} is now following {followDto.UserToFollow}");            
        }

        public IEnumerable<PostDto> DisplayWall(string username)
        {
            var wallMessages = new List<PostDto>();

            if (!_userFollowers.TryGetValue(username, out List<string>? followedUsers))
            {
                Console.WriteLine($"User {username} is not following any user");
                return Enumerable.Empty<PostDto>();
            }

            foreach (var followedUser in followedUsers)
            {
                if (_userMessages.TryGetValue(followedUser, out List<PostDto>? messages))
                {
                    wallMessages.AddRange(messages);
                }
            }

            return wallMessages.OrderByDescending(x => x.DateTime);
        }


    }
}

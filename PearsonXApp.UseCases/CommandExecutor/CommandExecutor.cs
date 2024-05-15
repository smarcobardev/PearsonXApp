using PearsonXApp.Entities;
using PearsonXApp.UseCases.Follow;
using PearsonXApp.UseCases.Post;
using PearsonXApp.UseCases.Users.Seeder;
using PearsonXApp.UseCases.Wall;

namespace PearsonXApp.UseCases.CommandExecutor
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly Dictionary<string, Action<string[]>> _commandHandlers;
        protected readonly List<User>? _users = [];

        private readonly IPostService _postService;
        private readonly IFollowService _followService;
        private readonly IWallService _wallService;

        public CommandExecutor(IPostService postService, IFollowService followService, IWallService wallService, IUserSeeder userSeeder)
        {
            _postService = postService;
            _followService = followService;
            _wallService = wallService;

            _commandHandlers = new Dictionary<string, Action<string[]>>
            {
                { "post", args => _postService.PostMessage(FormatPostArguments(args)) },
                { "follow", args => _followService.FollowUser(FormatFollowArguments(args), _users) },
                { "wall", args => _wallService.DisplayWall(args[(int)MessageHelper.UserIndex]) }
            };

            _users = userSeeder.SeedUsers();
        }

        public void ExecuteCommand(string[] commandParts)
        {
            ValidatePreLoadedUsers();

            string commandType = commandParts[(int)MessageHelper.ActionIndex].ToLower();

            if (!_commandHandlers.TryGetValue(commandType, out Action<string[]>? value))
            {
                Console.WriteLine("Invalid Command.");
                return;
            }

            if (!ValidateUser(ref commandParts))
            {
                Console.WriteLine($"User {commandParts[(int)MessageHelper.UserIndex]} not found.");
                return;
            }

            value(commandParts);
        }

        /// <summary>
        /// Validate if the user exists in the preloaded users
        /// </summary>
        /// <param name="commandParts"></param>
        /// <returns></returns>
        private bool ValidateUser(ref string[] commandParts)
        {
            return UserExists(ValidateUserFormat(ref commandParts, (int)MessageHelper.UserIndex));
        }

        /// <summary>
        /// Checks if there are users preloaded
        /// </summary>
        private void ValidatePreLoadedUsers()
        {
            if (_users is not { Count: > 0 })
            {
                Console.WriteLine("No users found.");
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Validate if the user exists in the preloaded users
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private bool UserExists(string user)
        {
            return _users?.Exists(u => u.Name.Equals(user, StringComparison.CurrentCultureIgnoreCase)) ?? false;
        }

        /// <summary>
        /// Adds @ to the user if it is not present
        /// </summary>
        /// <param name="user"></param>
        private static string ValidateUserFormat(ref string[] parameters, int index)
        {
            parameters[index] = parameters[index].StartsWith('@') ? parameters[index] : $"@{parameters[index]}";

            return parameters[index];
        }

        /// <summary>
        /// Formats the arguments for the follow command validating the user to follow format
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static FollowDto FormatFollowArguments(string[] args)
        {

            return new FollowDto { User = args[(int)MessageHelper.UserIndex], UserToFollow = ValidateUserFormat(ref args, (int)MessageHelper.UserToFollowIndex) };
        }


        /// <summary>
        /// Formats the arguments for the post command
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static PostDto FormatPostArguments(string[] args)
        {
            string user = args[(int)MessageHelper.UserIndex];
            string message = string.Join(" ", args, 2, args.Length - 2);

            return new PostDto { UserName = user, Message = message, DateTime = DateTime.UtcNow };
        }
    }

}

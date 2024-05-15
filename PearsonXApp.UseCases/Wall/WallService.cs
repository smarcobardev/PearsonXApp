using PearsonXApp.UseCases.Post;
using PearsonXApp.UseCases.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PearsonXApp.UseCases.Wall
{
    public class WallService(IUserService userService) : IWallService
    {
        public void DisplayWall(string username)
        {
            IEnumerable<PostDto> messages = userService.DisplayWall(username);

            foreach (var message in messages)
            {
                Console.WriteLine(FormatMessage(message));
            }
        }

        public string FormatMessage(PostDto message)
        {
            return $"{message.Message} {message.UserName} @{message.DateTime.ToLocalTime():HH:mm}";
        }
    }
}

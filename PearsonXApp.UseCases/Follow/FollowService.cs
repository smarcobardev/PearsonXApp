using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PearsonXApp.UseCases.Users;

namespace PearsonXApp.UseCases.Follow
{
    public class FollowService(IUserService userService) : IFollowService
    {
        public void FollowUser(FollowDto followParams, List<Entities.User> existingUsers)
        {
            if (!existingUsers.Exists(u => u.Name.Equals(followParams.UserToFollow, StringComparison.CurrentCultureIgnoreCase)))
            {
                Console.WriteLine($"Cannot follow user {followParams.UserToFollow}. User not found");
                return;
            }

            userService.FollowUser(followParams);
        }
    }
}

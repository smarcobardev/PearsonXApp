using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PearsonXApp.UseCases.Follow
{
    public interface IFollowService
    {
        void FollowUser(FollowDto followParams, List<Entities.User> existingUsers);
    }
}

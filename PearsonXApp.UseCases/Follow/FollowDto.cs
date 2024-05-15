using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PearsonXApp.UseCases.Follow
{
    public class FollowDto
    {
        public required string User { get; set; }

        public required string UserToFollow { get; set; }
    }
}

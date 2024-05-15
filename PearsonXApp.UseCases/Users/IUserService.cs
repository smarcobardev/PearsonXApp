using PearsonXApp.UseCases.Follow;
using PearsonXApp.UseCases.Post;

namespace PearsonXApp.UseCases.Users
{
    public interface IUserService
    {
        void PostMessage(PostDto postDto);

        void FollowUser(FollowDto followDto);

        IEnumerable<PostDto> DisplayWall(string username);
    }
}

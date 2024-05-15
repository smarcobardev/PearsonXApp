using PearsonXApp.UseCases.Users;

namespace PearsonXApp.UseCases.Post
{
    public class PostService(IUserService userService) : IPostService
    {
        public void PostMessage(PostDto postDto)
        {
            userService.PostMessage(postDto);
        }
    }
}

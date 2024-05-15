using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PearsonXApp.UseCases.Post
{
    public interface IPostService
    {
        void PostMessage(PostDto postDto);
    }
}

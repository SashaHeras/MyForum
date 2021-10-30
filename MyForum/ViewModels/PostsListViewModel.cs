using System.Linq;
using MyForum.Data.Models;

namespace MyForum.ViewModels
{
    public class PostsListViewModel
    {
        public IQueryable<Post> AllPosts { get; set; }
    }
}

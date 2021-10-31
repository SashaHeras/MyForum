using MyForum.Data.Models;

namespace MyForum.ViewModels
{
    public class PostViewModel
    {
        public Post GetPostByTopicId { get; set; }
        public string GetTopicNameById { get; set; }
    }
}

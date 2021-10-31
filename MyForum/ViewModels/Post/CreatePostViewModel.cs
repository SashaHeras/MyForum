namespace MyForum.ViewModels
{
    public class CreatePostViewModel
    {
        public string PostName { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int TopicId { get; set; }
        public int PostId { get; set; }
    }
}

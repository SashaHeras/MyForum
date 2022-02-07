namespace MyForum.ViewModels
{
    public class EditPostViewModel
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string PostName { get; set; }
        public string Description { get; set; }
        public int TopicId { get; set; }
    }
}

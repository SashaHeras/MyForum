namespace MyForum.Data.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string PostName { get; set; }
        public int TopicId { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }
}

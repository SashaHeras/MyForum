namespace MyForum.Data.Dto.Post
{
    public class FullPost
    {
        public int PostId { get; set; }
        public string PostName { get; set; }
        public string Description { get; set; }
        public int PostUserId { get; set; }
        public int PostTopicId { get; set; }
        public string PostUserName { get; set; }

        public FullPost()
        {

        }
    }
}

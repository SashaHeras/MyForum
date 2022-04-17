namespace MyForum.Data.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public int PostId { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }

        public bool IsAllow { get; set; }
    }
}

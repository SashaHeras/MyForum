namespace MyForum.Data.Models
{
    public class Coment
    {
        public int ComentId { get; set; }
        public string Comment { get; set; }
        public int PostId { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
    }
}

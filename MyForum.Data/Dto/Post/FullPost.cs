using System;

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
        public int Mark { get; set; }

        public FullPost()
        {

        }

        public FullPost(int p_id, string p_name, string p_desc, int p_userid, int p_tipocid, string p_username, int mark)
        {
            PostId = p_id;
            PostName = p_name;
            Description = p_desc;
            PostUserId = p_userid;
            PostTopicId = p_tipocid;
            PostUserName = p_username;
            Mark = mark;
        }
    }
}

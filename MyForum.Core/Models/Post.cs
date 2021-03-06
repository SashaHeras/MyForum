using System;
using System.ComponentModel.DataAnnotations;

namespace MyForum.Data.Models
{
    public class Post
    {
        public int PostId { get; set; }

        [Required]
        [Display(Name = "PostName")]
        public string PostName { get; set; }

        public int TopicId { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public int UserId { get; set; }

        public bool IsAllow { get; set; }

        public DateTime Updated { get; set; }

        public int Views { get; set; }
    }
}

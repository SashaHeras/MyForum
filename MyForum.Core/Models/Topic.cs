using System.ComponentModel.DataAnnotations;

namespace MyForum.Data.Models
{
    public class Topic
    {
        public int TopicId { get; set; }
        [Required]
        [Display(Name = "TopicName")]
        public string TopicName { get; set; }
    }
}

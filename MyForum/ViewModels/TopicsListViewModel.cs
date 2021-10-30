using System.Linq;
using MyForum.Data.Models;

namespace MyForum.ViewModels
{
    public class TopicsListViewModel
    {
        public IQueryable<Topic> AllTopics { get; set; }
    }
}

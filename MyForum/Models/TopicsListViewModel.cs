using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyForum.Controllers.Data.Models;

namespace MyForum.ViewModels
{
    public class TopicsListViewModel
    {
        public IQueryable<Topic> AllTopics { get; set; }
    }
}

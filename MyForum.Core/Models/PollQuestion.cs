using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyForum.Core.Models
{
    public class PollQuestion
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int PollId { get; set; }

        public int CountAnswers { get; set; }
    }
}

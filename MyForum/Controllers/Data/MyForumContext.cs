using Microsoft.EntityFrameworkCore;
using MyForum.Controllers.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Controllers.Data
{
    public class MyForumContext : DbContext
    {
        public DbSet<User> User { get; set; }

        public DbSet<Topic> Topic { get; set; }
        
        public DbSet<Post> Post { get; set; }

        public MyForumContext(DbContextOptions<MyForumContext> options):base(options)
        {

        }
    }
}

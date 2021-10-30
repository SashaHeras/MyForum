using Microsoft.EntityFrameworkCore;

namespace MyForum.Data.Models
{
    public class MyForumContext : DbContext
    {
        public DbSet<User> User { get; set; }

        public DbSet<Topic> Topic { get; set; }
        
        public DbSet<Post> Post { get; set; }

        public DbSet<Coment> Coment { get; set; }

        public MyForumContext(DbContextOptions<MyForumContext> options):base(options)
        {

        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace MultiplatformBot.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }


        public DbSet<VkConversation> VkConversations { get; set; }
    }
}
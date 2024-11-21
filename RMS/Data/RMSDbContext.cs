using Microsoft.EntityFrameworkCore;
using RMS.Models;

namespace RMS.Data
{
    public class RMSDbContext : DbContext
    {
        public RMSDbContext(DbContextOptions<RMSDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } // Existing
        public DbSet<Menu> Menus { get; set; } // Add this
    }
}

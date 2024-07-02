using Microsoft.EntityFrameworkCore;

namespace Case.Models
{
    /// <summary>
    ///     Creates a database context to store user data.
    /// </summary>
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) 
        {
        }

        public DbSet<User> Users { get; set; } = null;
    }
}

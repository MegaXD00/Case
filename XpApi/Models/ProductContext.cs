using Microsoft.EntityFrameworkCore;

namespace Case.Models
{
    /// <summary>
    ///     Creates a database context to store user data.
    /// </summary>
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null;
    }
}

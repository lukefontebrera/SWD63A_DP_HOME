using Microsoft.EntityFrameworkCore;

namespace WishlistAPI.Models
{
    public class WishListContext : DbContext
    {
        public WishListContext(DbContextOptions<WishListContext> options) : base(options) { }

        public DbSet<WishedMovie> WishedMovies { get; set; }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCMovieInfo.Models;

namespace MVCMovieInfo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MVCMovieInfo.Models.Genre> Genre { get; set; }
        public DbSet<MVCMovieInfo.Models.Movie> Movie { get; set; }
        public DbSet<MVCMovieInfo.Models.Favorite> Favorite { get; set; } = default!;
    }
}
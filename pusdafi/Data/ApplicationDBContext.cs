using Microsoft.EntityFrameworkCore;
using pusdafi.Models;

namespace pusdafi.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<Races> Race { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Address> Address { get; set; }

    }
}

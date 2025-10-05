using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Constants;
using RestaurantAPI.Models;

namespace RestaurantAPI.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admin { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Menu> Menu { get; set; }
        //public DbSet<ResturantConfig> ResturantConfiguration { get; set; }
        public DbSet<Table> Table { get; set; }



    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PriceChecker.DbProducts.Models;


namespace PriceChecker.DbProducts
{
    public class DbProductContext : DbContext
    {
        private static DbProductContext instance = null;
        static DbProductContext()
        {
            instance = new DbProductContext();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<PriceByDate> Prices { get; set; }

        private DbProductContext()
        {
            Database.EnsureCreated();
        }

        public static DbProductContext GetInstance()
        {
            return instance;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=data.db");
        }
    }
}
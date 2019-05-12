using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PriceChecker.DbProducts;
using System;


namespace PriceChecker.DbProducts
{
    public class DbProductContext : DbContext
    {
        public DbSet<ProductModel> Products { get; set; }
        private IConfiguration Configuration;

        public DbProductContext(IConfiguration configuration)
        {
            Configuration = configuration;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
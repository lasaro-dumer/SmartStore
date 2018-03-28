using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartStore.Data.Entities;

namespace SmartStore.Data
{
    public class SmartStoreDbContext : DbContext
    {
        private readonly IConfiguration _config;

        public SmartStoreDbContext(DbContextOptions options, IConfiguration configuration)
         : base(options)
        {
            _config = configuration;
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Product>()
              .Property(p => p.RowVersion)
              .ValueGeneratedOnAddOrUpdate()
              .IsConcurrencyToken();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config["ConnectionStrings:DefaultConnection"]);
        }
    }
}

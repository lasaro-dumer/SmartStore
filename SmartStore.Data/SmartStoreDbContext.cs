using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartStore.Data.Entities;

namespace SmartStore.Data
{
    public class SmartStoreDbContext : IdentityDbContext<UserEntity>
    {
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;
        public string ConnectionString { get; }

        public DbSet<Product> Products { get; set; }
        public DbSet<StockMovement> StockMoviments { get; set; }
        public DbSet<StockMovementType> StockMovementTypes { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public SmartStoreDbContext(DbContextOptions options, IConfiguration configuration, IHostingEnvironment env)
         : base(options)
        {
            _config = configuration;
            _env = env;

            if (_env.IsDevelopment())
            {
                ConnectionString = _config["ConnectionStrings:DefaultConnection"];
            }
            else
            {
                ConnectionString = _config["ENV_DBCONN"];
            }

            if (string.IsNullOrEmpty(ConnectionString))
                throw new Exception($"Connection string not configured for environment {_env.EnvironmentName}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
              .Property(p => p.RowVersion)
              .ValueGeneratedOnAddOrUpdate()
              .IsConcurrencyToken();

            modelBuilder.Entity<Tag>()
                 .HasIndex(u => u.Name)
                 .IsUnique();

            modelBuilder.Entity<ProductTag>()
                .HasKey(t => new { t.ProductId, t.TagId });

            modelBuilder.Entity<ProductTag>()
                .HasOne(pt => pt.Product)
                .WithMany("ProductTags");

            modelBuilder.Entity<ProductTag>()
                .HasOne(pt => pt.Tag)
                .WithMany("ProductTags");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}

using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace ShopLensWeb
{
    public class ShopLensContext : DbContext
    {
        private bool UseLocalDb { get; }
        private string ConnectionString { get; set; }
        
        public ShopLensContext(string connectionString, bool useLocalDb = false)
        {
            UseLocalDb = useLocalDb;
            ConnectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (UseLocalDb)
            {
                var dbFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var fileName = "ShopLens.db";
                var dbFullPath = Path.Combine(dbFolder, fileName);
                optionsBuilder.UseSqlite($"Filename={dbFullPath}");
            }
            else
            {
                var connectionString = ConnectionString;
                    
                optionsBuilder
                    .UseSqlServer(connectionString, providerOptions=>providerOptions.CommandTimeout(60))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
            
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<ShoppingSession> ShoppingSessions { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
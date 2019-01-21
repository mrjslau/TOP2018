using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace ShopLensWeb
{
    public class ShopLensContext : DbContext
    {
        private bool UseLocalDb { get; }
        private string ConnectionString { get; set; }

        /// <remarks>
        /// This constructor is needed so that migrations can be made.
        /// The migrations script looks for a constructor with no parameters.
        /// </remarks>
        public ShopLensContext()
            : this("", true)
        {
            
        }
        
        public ShopLensContext(string connectionString, bool useLocalDb)
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

using System;
using System.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShopLens;
using ShopLensWeb;

namespace MigrationsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var useLocalDb = Convert.ToBoolean(ConfigurationManager.AppSettings["UseLocalDb"]);
            var connectionString = ConfigurationManager.ConnectionStrings["DatabaseSource"].ConnectionString;

            using (var dbContext = new ShopLensContext(connectionString, useLocalDb))
            {
                dbContext.Database.Migrate();

                //DeleteAllUsers(dbContext);
                //var newUser = ShopLensRandomUserGenerator.GenerateRandomUser(Guid.NewGuid().ToString(), 14, 120);
                //AddNewUser(newUser);
                //UpdateDatabase(dbContext);
                //ListAllUsers(dbContext);
                //Console.ReadLine();
            }

        }

        public static void AddNewUser(ShopLensContext dbContext, User newUser)
        {
            dbContext.Users.Add(newUser);
        }

        public static void DeleteAllUsers(ShopLensContext dbContext)
        {
            Console.WriteLine("Deleting users...");

            var users = dbContext.Users.ToList();

            dbContext.Users.RemoveRange(users);
        }

        public static void UpdateDatabase(DbContext dbContext)
        {
            dbContext.SaveChanges();
        }

        public static void ListAllUsers(ShopLensContext dbContext)
        {
            Console.WriteLine("Listing all users...");

            var users = dbContext.Users.ToList();

            if (!users.Any())
            {
                Console.WriteLine("No users found.");
                return;
            }

            foreach (var user in users)
            {
                Console.WriteLine($"User {user.UserId}: Name - {user.Name}, BDay - {user.Birthday}");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShopLens;
using MigrationsConsole.Requirements;
using ShopLensWeb;

namespace MigrationsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var useLocalDb = Convert.ToBoolean(ConfigurationManager.AppSettings["UseLocalDb"]);
            var connectionString = ConfigurationManager.ConnectionStrings["DatabaseSource"].ConnectionString;
            ExecuteSqlRequirementQueries();

            using (var dbContext = new ShopLensContext(connectionString, useLocalDb))
            {
                dbContext.Database.Migrate();

                Console.WriteLine("Entity Framework queries:");
                //DeleteAllUsers(dbContext);
                //AddTestUsers(dbContext);
                ListAllUsers(dbContext);
            }

        }

        private static void ExecuteSqlRequirementQueries()
        {
            Console.WriteLine("======= ADO.NET queries: =======");
            var reqs = new SqlRequirements();
            reqs.Delete();
            reqs.Insert();
            reqs.Update();
            reqs.Select();
            reqs.DataAdapterDataTableAndLinq();
            Console.WriteLine("======= ADO.NET queries end. =======");
            Console.WriteLine();
        }

        public static void AddTestUsers(ShopLensContext dbContext)
        {
            Console.WriteLine("Adding users...");

            var user1 = new User { Name = "Marijus", 
                ShoppingSessions = new List<ShoppingSession>
                {
                    new ShoppingSession {Date = DateTime.UtcNow.AddDays(-1)},
                    new ShoppingSession {Date = DateTime.UtcNow}
                }};
            var user2 = new User { Name = "Marijus" };
            var user3 = new User { Name = "Tomus" };
            var user4 = new User { Name = "Edus" };
 
            var usersToAdd = new List<User> { user1, user2, user3, user4 };
            
            dbContext.Users.AddRange(usersToAdd);
            
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
            
            foreach(var user in users)
            {
                Console.WriteLine($"User {user.UserId}: Name - {user.Name}, BDay - {user.Birthday}") ;
            }
        }
        
        public static void DeleteAllUsers(ShopLensContext dbContext)
        {
            Console.WriteLine("Deleting users...");
            
            var users = dbContext.Users.ToList();
            
            dbContext.Users.RemoveRange(users);
            dbContext.SaveChanges();
        }
        
        public static void AddNewUser(ShopLensContext dbContext, User newUser)
        {
            dbContext.Users.Add(newUser);
        }
    }
}
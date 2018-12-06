using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RandomNameGenerator;
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
                //AddTestUsers(dbContext);
                //AddRandomUser(dbContext);
                ListAllUsers(dbContext);
            }

        }

        public static void AddTestUsers(ShopLensContext dbContext)
        {
            Console.WriteLine("Adding users...");

            var user1 = new User { Name = "Marijus" };
            var user2 = new User { Name = "Marijus" };
            var user3 = new User { Name = "Tomus" };
            var user4 = new User { Name = "Edus" };

            var usersToAdd = new List<User> { user1, user2, user3, user4 };

            dbContext.Users.AddRange(usersToAdd);

            dbContext.SaveChanges();
        }

        public static void AddRandomUser(ShopLensContext dbContext)
        {
            var userId = Guid.NewGuid();

            var randomNumberGenerator = new Random();

            var randomYear = randomNumberGenerator.Next(DateTime.Now.Year - 100, DateTime.Now.Year - 14);
            var randomMonth = randomNumberGenerator.Next(1, 13);
            var randomDay = randomNumberGenerator.Next(1, DateTime.DaysInMonth(randomYear, randomMonth) + 1);

            var randomBirthday = new DateTime(randomYear, randomMonth, randomDay);

            Gender userGender;
            int genderChance = randomNumberGenerator.Next(1, 101);

            if (genderChance >= 50) userGender = Gender.Female;

            else userGender = Gender.Male;

            var randomName = NameGenerator.GenerateFirstName(userGender);

            var userInfo = new User { Birthday = randomBirthday, Name = randomName, UserId = userId.ToString() };

            dbContext.Users.Add(userInfo);
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

        public static void DeleteAllUsers(ShopLensContext dbContext)
        {
            Console.WriteLine("Deleting users...");

            var users = dbContext.Users.ToList();

            dbContext.Users.RemoveRange(users);
            dbContext.SaveChanges();
        }
    }
}
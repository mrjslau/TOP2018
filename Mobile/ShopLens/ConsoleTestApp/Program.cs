using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var fileName = "ShopLens.db";
            var dbFullPath = Path.Combine(dbFolder, fileName);

            try
            {
                using (var dbContext = new ShopLensContext())
                {
                    dbContext.Database.Migrate();
                    
                    var user1 = new User { Name = "Marijus" };
                    var user2 = new User { Name = "Tomus" };
                    var user3 = new User { Name = "Edus" };
 
                    var usersToAdd = new List<User>() { user1, user2, user3 };
 
                    if (dbContext.Users.Count() < 3)
                    {
                        dbContext.Users.AddRange(usersToAdd);
                        dbContext.SaveChanges();
                    }
 
                    var usersAdded = dbContext.Users.ToList();
 
                    foreach(var user in usersAdded)
                    {
                        Console.WriteLine($"User {user.UserId}: Name - {user.Name}, BDay - {user.Birthday}") ;
                    }
                }
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
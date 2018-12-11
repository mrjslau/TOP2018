using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ShopLensWeb;

namespace MigrationsConsole.Requirements
{
    public class SqlRequirements
    {
        private readonly SqlConnection connection = new SqlConnection();
        private readonly Guid testUserId = Guid.Parse("aaaaad31-9864-4f24-e341-08d65dd61111");

        public SqlRequirements()
        {
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["DatabaseSource"].ConnectionString;
            connection.Open();
        }
        
        public void Delete()
        {
            var commandText = $"DELETE FROM [Users] WHERE UserId = '{testUserId}'";
            Console.WriteLine(commandText);
            
            var cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = commandText;
            cmd.ExecuteNonQuery();
        }
        
        public void Insert()
        {
            var commandText =
                $"INSERT INTO [Users] (UserId, Name, Birthday) VALUES('{testUserId}', 'Joe Smith', '01-Jan-01 12:00:00 AM')";
            Console.WriteLine(commandText);
            var cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = commandText;
            cmd.ExecuteNonQuery();
        }

        public void Select()
        {
            var commandText = "SELECT COUNT(*) FROM [Users]";
            Console.WriteLine(commandText);
            
            var cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = commandText;
            var userCount = cmd.ExecuteScalar();
            
            Console.WriteLine($"Count of users: {userCount}");
        }
        
        public void Update()
        {
            var commandText = $"UPDATE [Users] SET Name = 'Joline Smithy' WHERE UserId = '{testUserId}'";
            Console.WriteLine(commandText);
            
            var cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = commandText;
            cmd.ExecuteNonQuery();
        }

        public void DataAdapterDataTableAndLinq()
        {
            // DataAdapter, DataSet
            Console.WriteLine("Data adapter, data table, and linq:");
            var users = new List<User>();
            var sessions = new List<ShoppingSession>();
            
            var userAdapter = new SqlDataAdapter("SELECT UserId, Name FROM [Users]", connection);
            var userSet = new DataSet();
            
            userAdapter.Fill(userSet, "Users");
            userAdapter.Dispose();

            foreach (DataRow row in userSet.Tables[0].Rows)
            {
                Console.WriteLine($"Id = {row["UserId"]}, Name = {row["Name"]}");
                users.Add(new User{Name = (string) row["Name"], UserId = (Guid) row["UserId"]});
            }

            if (!users.Any())
            {
                Console.WriteLine("No users found. Please add some users to the database.");
                return;
            }
            
            var sessionAdapter = new SqlDataAdapter("SELECT ShoppingSessionId, UserId FROM [ShoppingSessions]", connection);
            var sessionSet = new DataSet();
            
            sessionAdapter.Fill(sessionSet, "ShoppingSessions");
            sessionAdapter.Dispose();
            
            foreach (DataRow row in sessionSet.Tables[0].Rows)
            {
                sessions.Add(new ShoppingSession {ShoppingSessionId = (Guid) row["ShoppingSessionId"], UserId = (Guid) row["UserId"]});
            }
            
            // LINQ queries
            // Aggregate
            var userNamesAggregated = users.Select(x => x.Name).Aggregate((x, y) => $"{x} and {y}");
            Console.WriteLine("(.Aggregate)User names aggregated: " + userNamesAggregated);

            // Take
            var firstTwoUsers = users.Take(3).ToList();
            Console.WriteLine("User names .Take(3): " + string.Join(", ", firstTwoUsers.Select(x => x.Name)));

            // GroupBy
            var userNamesGrouped = users.GroupBy(x => x);
            var mostCommonName = userNamesGrouped.First(x => x.Count() == userNamesGrouped.Max(y => y.Count())).Key;
            Console.WriteLine($"(.GroupBy) Most common username: {mostCommonName}");

            // Join
            var usersAndShoppingSessions = sessions
                .Join(users, x => x.UserId, x => x.UserId, (x, y) => new {x.ShoppingSessionId, x.UserId, y.Name})
                .Select(x => $"Shopping session of {x.Name}({x.UserId}): \t {x.ShoppingSessionId}");

            Console.WriteLine("(.Join) Users and their shopping sessions:");
            foreach (var userAndShoppingSession in usersAndShoppingSessions)
            {
                Console.WriteLine(userAndShoppingSession);
            }
        }
    }
}

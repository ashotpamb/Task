using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using TaskLogix.Data;
using TaskLogix.Models;

namespace TaskLogix.Seeders
{
    public static class AdminSeeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                var hashPassword = new PasswordHasher<string>();
                string hashedPassword = hashPassword.HashPassword(null, "admin1234");
                var admin = new User
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "admin@gmail.com",
                    Password = hashedPassword,
                    Role = Roles.Admin
                };
                dbContext.Users.Add(admin);
                dbContext.SaveChanges();
            }
        }

    }
}
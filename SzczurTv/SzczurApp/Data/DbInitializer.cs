using SzczurApp.Data;
using SzczurApp.Data.Models;

namespace SzczurApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Look for any students.
            if (context.Users.Any())
            {
                return;
            }

            var users = new User[]
            {
                new User{Username="Ben10", Email="ben@ben.ben", PasswordHash="ben10", DateOfBirth=DateTime.Parse("2001-04-20")},
                new User{Username="admin", Email="admin@example.com", PasswordHash="admin", DateOfBirth=DateTime.Parse("2000-01-01")}
            };
            
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}

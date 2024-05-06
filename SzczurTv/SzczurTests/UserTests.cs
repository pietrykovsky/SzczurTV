using System.ComponentModel.DataAnnotations;
using SzczurApp.Data; // Assuming this namespace contains ApplicationDbContext and ApplicationUser

namespace SzczurTests.Users
{
    public class UserModelTests : UserTestBase
    {
        [Fact]
        public void Create_ValidUser_UserIsAdded()
        {
            using (var context = CreateContext())
            {
                var user = new ApplicationUser
                {
                    UserName = "newuser", // Identity uses UserName, not Username
                    Email = "newuser@example.com",
                    PasswordHash = "securehash"
                };
                context.Add(user);
                context.SaveChanges();

                Assert.True(context.Users.Any(u => u.UserName == "newuser"));
            }
        }

        [Theory]
        [InlineData("", "email@example.com", "hash")] // Empty username
        [InlineData("username", "", "hash")] // Empty email
        [InlineData("username", "notanemail", "hash")] // Invalid email format
        public void Create_InvalidUser_ShouldFailValidation(string username, string email, string password)
        {
            var user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                PasswordHash = password
            };
            List<ValidationResult> results;
            bool isValid = ValidateModel(user, out results);

            Assert.False(isValid); // Assert that the model is not valid
            Assert.NotEmpty(results); // Assert that there are validation messages
        }

        [Fact]
        public void Read_UserExists_ByUsername_ReturnsCorrectUser()
        {
            using (var context = CreateContext())
            {
                var user = new ApplicationUser
                {
                    UserName = "readuser",
                    Email = "readuser@example.com",
                    PasswordHash = "readhash"
                };
                context.Users.Add(user);
                context.SaveChanges();

                var retrievedUser = context.Users.FirstOrDefault(u => u.UserName == "readuser");
                Assert.NotNull(retrievedUser);
                Assert.Equal("readuser@example.com", retrievedUser.Email);
            }
        }

        [Fact]
        public void Read_NoUserExists_ReturnsNull()
        {
            using (var context = CreateContext())
            {
                var retrievedUser = context.Users.FirstOrDefault(u => u.UserName == "nonexistentuser");
                Assert.Null(retrievedUser);
            }
        }

        [Fact]
        public void Update_UserExists_UpdatesUserSuccessfully()
        {
            using (var context = CreateContext())
            {
                var user = new ApplicationUser
                {
                    UserName = "updateuser",
                    Email = "updateuser@example.com",
                    PasswordHash = "updatehash"
                };
                context.Users.Add(user);
                context.SaveChanges();

                user.Email = "updateduser@example.com";
                context.Users.Update(user);
                context.SaveChanges();

                var updatedUser = context.Users.FirstOrDefault(u => u.UserName == "updateuser");
                Assert.Equal("updateduser@example.com", updatedUser.Email);
            }
        }

        [Fact]
        public void Delete_UserExists_UserIsDeleted()
        {
            using (var context = CreateContext())
            {
                var user = new ApplicationUser
                {
                    UserName = "deleteuser",
                    Email = "deleteuser@example.com",
                    PasswordHash = "deletehash"
                };
                context.Users.Add(user);
                context.SaveChanges();

                context.Users.Remove(user);
                context.SaveChanges();

                var deletedUser = context.Users.FirstOrDefault(u => u.UserName == "deleteuser");
                Assert.Null(deletedUser);
            }
        }
    }
}

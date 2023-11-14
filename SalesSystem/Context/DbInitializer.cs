using SalesSystem.Models;
using static SalesSystem.Models.Roles;

namespace SalesSystem.Context
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDBContext context)
        {
            context.Database.EnsureCreated();
            if (!context.Users.Any())
            {
                UserModel defaultUser = new()
                {
                    FirstName = "Kevin",
                    LastName = "Vilca",
                    Email = "keviv1q2@gmail.com",
                    Age = 22,
                    Gender = "M",
                    Username = "keevsz",
                    Password = "password",
                    Role= RoleType.ADMIN
                };

                context.Users.Add(defaultUser);
                context.SaveChanges();
            }
        }
    }
}

using SalesSystem.Models;
using System.Xml.Linq;
using static SalesSystem.Models.Roles;

namespace SalesSystem.Context
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDBContext context)
        {
            context.Database.EnsureCreated();
            string password = BCrypt.Net.BCrypt.HashPassword("P@ssw0rd1234");

            if (!context.Roles.Any())
            {
                var defaultRoles = new[]
             {
                new Role { Name = "ADMIN", Description = "Administrator role with full access." },
                new Role{ Name = "USER", Description = "Standard user role with basic access." }
            };

                context.Roles.AddRange(defaultRoles);
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                Role adminRol = context.Roles.FirstOrDefault(rol=>rol.Name == "ADMIN")!;
                UserModel defaultUser = new()
                {
                    FirstName = "Kevin",
                    LastName = "Vilca",
                    Email = "keviv1q2@gmail.com",
                    Age = 22,
                    Gender = "M",
                    Username = "keevsz",
                    Password = password,
                    Role= adminRol
                };

                context.Users.Add(defaultUser);
                context.SaveChanges();
            }
        }
    }
}

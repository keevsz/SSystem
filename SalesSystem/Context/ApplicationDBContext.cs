using Microsoft.EntityFrameworkCore;
using SalesSystem.Models;

namespace SalesSystem.Context
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {

        }
        public DbSet<UserModel> Users { get; set; }
    }
}

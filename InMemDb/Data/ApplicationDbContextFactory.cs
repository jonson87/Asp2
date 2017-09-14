using InMemDb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace InMemDb.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer("Server=tcp:bennyspizzaserver.database.windows.net,1433;" +
                                 "Initial Catalog=BennysPizzaDb;Persist Security Info=False;" +
                                 "User ID=benny@BennysPizzaDb;Password=Javisst7;" +
                                 "MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            return new ApplicationDbContext(builder.Options);
        }
    }
}

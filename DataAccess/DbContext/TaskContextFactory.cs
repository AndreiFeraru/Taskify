using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.DatabaseContext
{
    public class TaskContextFactory: IDesignTimeDbContextFactory<TaskDbContext>
    {
        public TaskDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TaskDbContext>();
            // optionsBuilder.UseSqlite();

            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            return new TaskDbContext(optionsBuilder.Options);
        }
    }
}

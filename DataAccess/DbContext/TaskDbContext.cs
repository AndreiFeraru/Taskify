using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DatabaseContext;

public class TaskDbContext: DbContext
{
    public TaskDbContext() { }

    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Set Id as read-only
        modelBuilder.Entity<TaskItem>().Property(t => t.Id).IsRequired();

        // Set Title as required and non-nullable
        modelBuilder.Entity<TaskItem>().Property(t => t.Title).IsRequired();

        base.OnModelCreating(modelBuilder);
    }
}

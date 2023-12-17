using DataAccess.DatabaseContext;
using TaskStatus = DataAccess.Models.TaskStatus;

namespace Server.Services;

public class TaskStatusUpdaterService(IServiceProvider serviceProvider, TimeSpan runInterval): BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TaskDbContext>();

                var tasksToUpdate = dbContext.Tasks
                    .AsEnumerable()
                    .Where(task => task.DueDate < DateTimeOffset.UtcNow &&
                        task.Status != TaskStatus.Overdue &&
                        task.Status != TaskStatus.Completed);

                foreach (var task in tasksToUpdate)
                {
                    if (task.Status == TaskStatus.Pending)
                    {
                        task.Status = TaskStatus.Overdue;
                    }
                    else if (task.Status == TaskStatus.InProgress)
                    {
                        task.Status = TaskStatus.Pending;
                    }
                }

                await dbContext.SaveChangesAsync(); // Save changes to the database
            }

            await Task.Delay(runInterval, stoppingToken); // Delay for 24 hours before the next iteration
        }
    }

    // For testing purposes
    public async Task ExecuteAsyncPublic(CancellationToken cancellationToken)
        => ExecuteAsync(cancellationToken);

}

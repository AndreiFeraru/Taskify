using Microsoft.EntityFrameworkCore;
using DataAccess.DatabaseContext;
using DataAccess.Models;

namespace Server.Services;

public class TaskService(TaskDbContext dbContext): ITaskService
{
    public async Task<IEnumerable<TaskItem>> GetTasks()
    {
        return await dbContext.Tasks.ToListAsync();
    }

    public async Task<TaskItem> GetTaskById(int taskId)
    {
        return await dbContext.Tasks.FindAsync(taskId) ?? 
            throw new InvalidOperationException();
    }

    public async Task<TaskItem> CreateTask(TaskItem task)
    {
        int maxId = await dbContext.Tasks.AnyAsync() ? dbContext.Tasks.Max(t => t.Id) : 0;
        task.Id = maxId + 1;
        dbContext.Tasks.Add(task);
        await dbContext.SaveChangesAsync();
        return task;
    }

    public async Task<TaskItem> UpdateTask(TaskItem task)
    {
        dbContext.Tasks.Update(task);
        await dbContext.SaveChangesAsync();
        return task;
    }

    public async Task DeleteTask(int taskId)
    {
        var task = await dbContext.Tasks.FindAsync(taskId);
        if(task is not null)
        {
            dbContext.Tasks.Remove(task);
            await dbContext.SaveChangesAsync();
        }
    }
}

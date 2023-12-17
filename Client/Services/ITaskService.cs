using Client.Models;

namespace Client.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskItem>> GetTasks();
    Task<TaskItem> GetTask(int id);
    Task UpdateTask(TaskItem task);
    Task CreateTask(TaskItem task);
    Task DeleteTask(int id);
}

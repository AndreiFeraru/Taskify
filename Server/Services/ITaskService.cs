using DataAccess.Models;

namespace Server.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskItem>> GetTasks();
    Task<TaskItem> GetTaskById(int id);
    Task<TaskItem> UpdateTask(TaskItem task);
    Task<TaskItem> CreateTask(TaskItem task);
    Task DeleteTask(int id);
}
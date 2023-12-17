using Microsoft.EntityFrameworkCore;
using DataAccess.DatabaseContext;
using Server.Services;
using DataAccess.Models;

namespace Server.Tests.Services
{
    public class TaskServiceTests: IDisposable
    {
        private readonly DbContextOptions<TaskDbContext> _options = new DbContextOptionsBuilder<TaskDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        private TaskDbContext _context;

        public TaskServiceTests()
        {
            _context = new TaskDbContext(_options);

            _context.Tasks.AddRange(
                new TaskItem { Title = "Task 1" },
                new TaskItem { Title = "Task 2" }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task Test_GetTasks_ReturnsZeroTasks()
        {
            // Arrange
            _context = new TaskDbContext(_options);
            _context.RemoveRange(_context.Tasks);
            _context.SaveChanges();
            var service = new TaskService(_context);

            // Act
            var tasks = await service.GetTasks();

            // Assert
            Assert.Empty(tasks);
        }

        [Fact]
        public async Task Test_GetTasks_ReturnsAllTasks()
        {
            // Arrange
            var service = new TaskService(_context);

            // Act
            var tasks = await service.GetTasks();

            // Assert
            Assert.Equal(2, tasks.Count());
        }

        [Fact]
        public async Task Test_GetTaskById_ReturnsCorrectTask()
        {
            // Arrange
            var service = new TaskService(_context);
            var taskId = 1;

            // Act
            var task = await service.GetTaskById(taskId);

            // Assert
            Assert.NotNull(task);
            Assert.Equal(taskId, task.Id);
        }

        [Fact]
        public async Task Test_GetTaskById_NonExistingTask()
        {
            // Arrange
            var service = new TaskService(_context);
            var taskId = 999;

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetTaskById(taskId));
        }

        [Fact]
        public async Task Test_CreateTask_IncreasesTaskCount()
        {
            // Arrange
            var service = new TaskService(_context);
            var initialTaskCount = _context.Tasks.Count();
            var newTask = new TaskItem { Title = "New Task" };

            // Act
            var createdTask = await service.CreateTask(newTask);
            var tasksAfterCreate = await service.GetTasks();

            // Assert
            Assert.NotNull(createdTask);
            Assert.Equal(initialTaskCount + 1, tasksAfterCreate.Count());
            Assert.Contains(newTask, tasksAfterCreate);
        }

        [Fact]
        public async Task Test_CreateTask_ExistingTitle()
        {
            // Arrange
            var service = new TaskService(_context);
            var initialTaskCount = _context.Tasks.Count();
            var newTask1 = new TaskItem { Title = "New Task" };
            var newTask2 = new TaskItem { Title = "New Task" };

            // Act
            var createdTask1 = await service.CreateTask(newTask1);
            var createdTask2 = await service.CreateTask(newTask2);
            var tasksAfterCreate = await service.GetTasks();

            // Assert
            Assert.NotNull(createdTask1);
            Assert.NotNull(createdTask2);
            Assert.Equal(initialTaskCount + 2, tasksAfterCreate.Count());
            Assert.Contains(newTask1, tasksAfterCreate);
            Assert.Equal(2, tasksAfterCreate.Count(t => t.Title == newTask1.Title));
        }

        [Fact]
        public async Task Test_CreateTask_EmptyTitle()
        {
            // Arrange
            var service = new TaskService(_context);
            var initialTaskCount = _context.Tasks.Count();
            var emptyTitleTask = new TaskItem { Title = "" };

            // Act
            var createdTask = await service.CreateTask(emptyTitleTask);
            var tasksAfterCreate = await service.GetTasks();

            // Assert
            Assert.NotNull(createdTask);
            Assert.Equal(initialTaskCount + 1, tasksAfterCreate.Count());
            Assert.Contains(emptyTitleTask, tasksAfterCreate);
        }

        [Fact]
        public async Task Test_CreateTask_NullTitle()
        {
            // Arrange
            var service = new TaskService(_context);
            var emptyTitleTask = new TaskItem { Title = null };

            // Act
            // Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => service.CreateTask(emptyTitleTask));
        }

        [Fact]
        public async Task Test_UpdateTask_ChangesTaskDetails()
        {
            // Arrange
            var service = new TaskService(_context);
            var taskId = 1;
            var updatedTitle = "Updated Task Title";

            // Act
            var taskToUpdate = await service.GetTaskById(taskId);
            taskToUpdate.Title = updatedTitle;
            var updatedTask = await service.UpdateTask(taskToUpdate);

            // Assert
            Assert.NotNull(updatedTask);
            Assert.Equal(updatedTitle, updatedTask.Title);
        }

        [Fact]
        public async Task Test_DeleteTask_RemovesTaskFromList()
        {
            // Arrange
            var service = new TaskService(_context);
            var taskId = 1;

            // Act
            await service.DeleteTask(taskId);
            var tasksAfterDelete = await service.GetTasks();

            // Assert
            Assert.DoesNotContain(tasksAfterDelete, t => t.Id == taskId);
        }

        public void Dispose()
        {
            _context.Dispose();
            _context = new TaskDbContext(_options);
            _context.Database.EnsureDeleted(); // Remove the database
            _context.Database.EnsureCreated(); // Recreate the database
        }

    }
}

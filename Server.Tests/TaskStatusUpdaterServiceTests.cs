using System;
using Moq;
using DataAccess.DatabaseContext;
using DataAccess.Models;
using Server.Services;
using Microsoft.EntityFrameworkCore;
using TaskStatus = DataAccess.Models.TaskStatus;

namespace Server.Tests.Services
{
    public class TaskStatusUpdaterServiceTests
    {
        public TaskStatusUpdaterServiceTests()
        {
        }

        [Fact]
        public async Task ExecuteAsync_ShouldUpdateTaskStatus_WhenDueDateIsPast()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new() { Title = "t1", DueDate = DateTimeOffset.UtcNow.AddDays(-1), Status = TaskStatus.Pending },
                new() { Title = "t2", DueDate = DateTimeOffset.UtcNow.AddDays(-1), Status = TaskStatus.InProgress },
                new() { Title = "t3", DueDate = DateTimeOffset.UtcNow.AddDays(1), Status = TaskStatus.Pending }
            };

            var taskDbContext = new TaskDbContext(new DbContextOptionsBuilder<TaskDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options);
            taskDbContext.Tasks.AddRange(tasks);
            taskDbContext.SaveChanges();
            
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(x => x.GetService(typeof(TaskDbContext))).Returns(taskDbContext);

            var service = new TaskStatusUpdaterService(serviceProvider.Object, TimeSpan.FromHours(24));

            // Act
            await service.ExecuteAsyncPublic(CancellationToken.None);

            // Assert
            Assert.Equal(TaskStatus.Overdue, tasks[0].Status);
            Assert.Equal(TaskStatus.Pending, tasks[1].Status);
            Assert.Equal(TaskStatus.Pending, tasks[2].Status);
        }
    }
}

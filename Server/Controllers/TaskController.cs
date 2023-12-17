using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers;

[Route("api/tasks")]
[ApiController]
public class TaskController(ITaskService taskService, ILogger<TaskController> logger) : ControllerBase
{

    // GET: api/tasks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
    {
        var tasks = await taskService.GetTasks();

        if(tasks is null)
        {
            return NotFound();
        }

        return Ok(tasks);
    }

    // GET: api/tasks/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetTask(int id)
    {
        try
        {
            var task = await taskService.GetTaskById(id);
            return Ok(task);
        }
        catch(InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Error getting task");
            return StatusCode(500, "Error getting task");
        }
    }

    // POST: api/tasks
    [HttpPost]
    public async Task<ActionResult<TaskItem>> PostTask(TaskItem task)
    {
        var returnedTask = await taskService.CreateTask(task);
        return CreatedAtAction("GetTask", new { id = returnedTask.Id }, returnedTask);
    }

    // PUT: api/tasks/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTask(int id, TaskItem task)
    {
        if (id != task.Id)
        {
            logger.LogError("Error updating task. Id from URI is different from task Id.");
            return BadRequest();
        }

        var returnedTask = await taskService.UpdateTask(task);
        return Ok(returnedTask);
    }

    // DELETE: api/tasks/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        await taskService.DeleteTask(id);
        return NoContent();
    }
}

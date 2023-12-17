using System.ComponentModel.DataAnnotations;

namespace Client.Models;

public class TaskItem
{
    [Required]
    public int Id { get;  set; }
    [Required, MinLength(8)]
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset DueDate { get; set; }
    public TaskItemStatus Status { get; set; }
}

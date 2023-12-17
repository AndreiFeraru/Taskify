using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public class TaskItem
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Title is required")]
    [MinLength(1, ErrorMessage = "Title should not be empty")]
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset DueDate { get; set; }
    public TaskStatus Status { get; set; }
}
using System.Text;
using System.Text.Json;
using Client.Models;

namespace Client.Services;

public class TaskService(IHttpClientFactory httpClientFactory) : ITaskService
{
    private readonly HttpClient _client = httpClientFactory.CreateClient("Taskify.Server");
    private readonly JsonSerializerOptions _serializeOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    private const string TASK_CONTROLLER_ROUTE = "tasks";

    public async Task<IEnumerable<TaskItem>> GetTasks()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_client.BaseAddress}/{TASK_CONTROLLER_ROUTE}");

        var response = await _client.SendAsync(request);

        if (response.IsSuccessStatusCode == false)
        {
            throw new ApplicationException($"Error getting tasks: {response.ReasonPhrase}");
        }

        var responseStream = await response.Content.ReadAsStreamAsync();
        var tasks = await JsonSerializer.DeserializeAsync<IEnumerable<TaskItem>>(responseStream, _serializeOptions);
        return tasks ?? Enumerable.Empty<TaskItem>();
    }

    public async Task<TaskItem> GetTask(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_client.BaseAddress}/{TASK_CONTROLLER_ROUTE}/{id}");

        var response = await _client.SendAsync(request);

        if (response.IsSuccessStatusCode == false)
        {
            throw new ApplicationException($"Error getting task: {response.ReasonPhrase}");
        }

        var responseStream = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<TaskItem>(responseStream, _serializeOptions)
            ?? throw new ApplicationException("Error getting task: Task is null");
    }

    public async Task CreateTask(TaskItem task)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_client.BaseAddress}/{TASK_CONTROLLER_ROUTE}")
        {
            Content = new StringContent(JsonSerializer.Serialize(task, _serializeOptions), Encoding.UTF8, "application/json")
        };

        var response = await _client.SendAsync(request);
        if(response.IsSuccessStatusCode == false)
        {                 
            throw new ApplicationException($"Could not create task: {response.ReasonPhrase}");
        }
        var responseStream = await response.Content.ReadAsStreamAsync();
        await JsonSerializer.DeserializeAsync<TaskItem>(responseStream, _serializeOptions);
    }

    public async Task UpdateTask(TaskItem task)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"{_client.BaseAddress}/{TASK_CONTROLLER_ROUTE}/{task.Id}")
        {
            Content = new StringContent(JsonSerializer.Serialize(task, _serializeOptions), Encoding.UTF8, "application/json")
        };

        var response = await _client.SendAsync(request);

        if (response.IsSuccessStatusCode == false)
        {
            throw new ApplicationException($"Could not update task: {response.ReasonPhrase}");
        }
        
        var responseStream = await response.Content.ReadAsStreamAsync();
        await JsonSerializer.DeserializeAsync<TaskItem>(responseStream, _serializeOptions);
    }

    public async Task DeleteTask(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"{_client.BaseAddress}/{TASK_CONTROLLER_ROUTE}/{id}");

        var response = await _client.SendAsync(request);

        if (response.IsSuccessStatusCode == false)
        {
            throw new ApplicationException($"Could not delete task: {response.ReasonPhrase}");
        }
    }
}

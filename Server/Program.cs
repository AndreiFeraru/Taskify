using Microsoft.EntityFrameworkCore;
using DataAccess.DatabaseContext;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped(provider => new TaskContextFactory().CreateDbContext([]));

builder.Services.AddScoped<ITaskService, TaskService>();

// TODO: Change service to take run interval from configuration
builder.Services.AddHostedService(provider => new TaskStatusUpdaterService(provider, TimeSpan.FromHours(24))); 

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

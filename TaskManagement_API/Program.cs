using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.SwaggerGen;
using TaskManager_API.Repositories;
using TaskManager_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<ExamplesOperationFilter>();
    c.UseInlineDefinitionsForEnums();
}
    );

builder.Services.AddSingleton<IMongoClient>(provider =>
{
    var mongoConnectionString = builder.Configuration.GetConnectionString("MongoAtlasConnection");
    return new MongoClient(mongoConnectionString);
});

builder.Services.AddScoped<IMongoDatabase>(provider =>
{
    var client = provider.GetRequiredService<IMongoClient>();
    return client.GetDatabase("TaskManager");
});
// Configure CORS to allow any origin.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAnyOrigin");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public class ExamplesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var response in operation.Responses.Values)
        {
            if (response.Content != null && response.Content.Any())
            {
                foreach (var mediaType in response.Content.Values)
                {
                    if (mediaType.Schema != null)
                    {
                        mediaType.Schema.Default = null;
                        mediaType.Schema.Example = null;
                        mediaType.Schema.Enum = null;
                        mediaType.Schema.Reference = null;
                        mediaType.Schema.Properties = null;
                    }
                }
            }
        }
    }
}

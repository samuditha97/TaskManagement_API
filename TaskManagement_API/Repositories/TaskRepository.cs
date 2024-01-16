using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using TaskManager_API.Models;

namespace TaskManager_API.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Tasks>> GetTasks();
        Task<Tasks> GetTaskById(string id);
        Task<Tasks> CreateTask(Tasks task);
        Task<bool> UpdateTask(string id, Tasks task);
        Task<bool> DeleteTask(string id);
    }

    public class TaskRepository : ITaskRepository
    {
        private readonly IMongoCollection<Tasks> _tasks;
        private readonly ILogger<TaskRepository> _logger;

        public TaskRepository(IMongoDatabase database, ILogger<TaskRepository> logger)
        {
            _tasks = database.GetCollection<Tasks>("Tasks");
            _logger = logger;
        }

        public async Task<Tasks> CreateTask(Tasks task)
        {
            try
            {
                if (task == null)
                {
                    throw new ArgumentNullException(nameof(task), "Task object is null.");
                }

                _logger.LogInformation($"CreateTask called with TaskId: {task.Id}");

                var validationContext = new ValidationContext(task, null, null);
                Validator.ValidateObject(task, validationContext, true);

                await _tasks.InsertOneAsync(task);
                return task;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateTask");
                throw;
            }
        }

        public async Task<IEnumerable<Tasks>> GetTasks()
        {
            try
            {
                _logger.LogInformation("GetTasks called");

                return await _tasks.Find(t => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTasks");
                throw;
            }
        }

        public async Task<Tasks> GetTaskById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentException("Id cannot be null or empty.", nameof(id));
                }

                _logger.LogInformation($"GetTaskById called with Id: {id}");

                return await _tasks.Find(t => t.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTaskById");
                throw;
            }
        }

        public async Task<bool> UpdateTask(string id, Tasks task)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentException("Id cannot be null or empty.", nameof(id));
                }

                if (task == null)
                {
                    throw new ArgumentNullException(nameof(task), "Task object is null.");
                }

                _logger.LogInformation($"UpdateTask called with Id: {id}");

                var result = await _tasks.ReplaceOneAsync(t => t.Id == id, task);
                return result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateTask");
                throw;
            }
        }

        public async Task<bool> DeleteTask(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentException("Id cannot be null or empty.", nameof(id));
                }

                _logger.LogInformation($"DeleteTask called with Id: {id}");

                var result = await _tasks.DeleteOneAsync(t => t.Id == id);
                return result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteTask");
                throw;
            }
        }

        

       
    }
}

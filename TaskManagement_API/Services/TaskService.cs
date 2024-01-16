using System;
using TaskManager_API.Models;
using TaskManager_API.Repositories;

namespace TaskManager_API.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<Tasks>> GetTasks();
        Task<Tasks> GetTaskById(string id);
        Task<Tasks> CreateTask(Tasks task);
        Task<bool> UpdateTask(string id, Tasks task);
        Task<bool> DeleteTask(string id);
    }
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public Task<Tasks> CreateTask(Tasks task)
        {
            return _taskRepository.CreateTask(task);
        }

        public Task<bool> DeleteTask(string id)
        {
            return _taskRepository.DeleteTask(id);
        }

        public Task<Tasks> GetTaskById(string id)
        {
            return _taskRepository.GetTaskById(id);
        }

        public Task<IEnumerable<Tasks>> GetTasks()
        {
            return _taskRepository.GetTasks();
        }

        public Task<bool> UpdateTask(string id, Tasks task)
        {
            return _taskRepository.UpdateTask(id, task);
        }
    }
}

using System;
using Microsoft.AspNetCore.Mvc;
using TaskManager_API.Models;
using TaskManager_API.Services;

namespace TaskManager_API.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskService taskService, ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Task>> CreateTask(Tasks task)
        {
            try
            {
                var createdTask = await _taskService.CreateTask(task);
                return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateTask: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasks()
        {
            try
            {
                var tasks = await _taskService.GetTasks();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetTasks: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Task>> GetTaskById(string id)
        {
            try
            {
                var task = await _taskService.GetTaskById(id);
                if (task == null)
                {
                    return NotFound();
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetTaskById: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(string id, Tasks task)
        {
            try
            {
                var result = await _taskService.UpdateTask(id, task);
                if (result)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateTask: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            try
            {
                var result = await _taskService.DeleteTask(id);
                if (result)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteTask: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}

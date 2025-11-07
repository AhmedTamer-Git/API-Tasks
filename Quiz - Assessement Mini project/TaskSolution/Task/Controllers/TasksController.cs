using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Get all tasks with optional filtering and sorting
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll(
            [FromQuery] bool? isCompleted = null,
            [FromQuery] string? sortBy = null)
        {
            var tasks = await _taskService.GetFilteredTasksAsync(isCompleted, sortBy);
            return Ok(tasks);
        }

        /// <summary>
        /// Get a task by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound($"Task with ID {id} not found.");

            return Ok(task);
        }

        /// <summary>
        /// Create a new task
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TaskItem>> Create(TaskItem task)
        {
            try
            {
                var createdTask = await _taskService.CreateTaskAsync(task);
                return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update an existing task
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<TaskItem>> Update(int id, TaskItem task)
        {
            if (id != task.Id)
                return BadRequest("Task ID mismatch.");

            try
            {
                var updatedTask = await _taskService.UpdateTaskAsync(task);
                if (updatedTask == null)
                    return NotFound($"Task with ID {id} not found.");

                return Ok(updatedTask);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id);
            if (!result)
                return NotFound($"Task with ID {id} not found.");

            return NoContent();
        }
    }
}

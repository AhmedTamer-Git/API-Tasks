using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.TaskManagement
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem task)
        {
            // Business logic: DueDate must be in the future
            if (task.DueDate.HasValue && task.DueDate.Value < DateTime.UtcNow)
            {
                throw new InvalidOperationException("DueDate must be in the future.");
            }

            return await _repository.CreateAsync(task);
        }

        public async Task<TaskItem?> UpdateTaskAsync(TaskItem task)
        {
            // Business logic: DueDate must be in the future
            if (task.DueDate.HasValue && task.DueDate.Value < DateTime.UtcNow)
            {
                throw new InvalidOperationException("DueDate must be in the future.");
            }

            return await _repository.UpdateAsync(task);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<TaskItem>> GetFilteredTasksAsync(bool? isCompleted, string? sortBy)
        {
            return await _repository.GetFilteredTasksAsync(isCompleted, sortBy);
        }
    }
}

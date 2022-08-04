using System;
using csharp_it.Models;
using Task = csharp_it.Models.Task;

namespace csharp_it.Services.Interfaces
{
	public interface ITaskService
	{
        Task<Task> CreateTaskAsync(Task question);
        System.Threading.Tasks.Task DeleteAsync(Task task);
        Task<Task> GetTaskByIdAsync(int id);
        Task<IEnumerable<Task>> GetTasksByLessonIdAsync(int lessonId);
        Task<Task> UpdateTaskAsync(Task task);
    }
}


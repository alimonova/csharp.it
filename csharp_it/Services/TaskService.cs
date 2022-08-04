using System;
using csharp_it.Models;
using csharp_it.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Task = csharp_it.Models.Task;

namespace csharp_it.Services
{
	public class TaskService : ITaskService
    {
        private readonly Models.DbContext _dbcontext;

        public TaskService(Models.DbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<Task> CreateTaskAsync(Task task)
        {
            await _dbcontext.Tasks.AddAsync(task);
            await _dbcontext.SaveChangesAsync();

            return task;
        }

        public async System.Threading.Tasks.Task DeleteAsync(Task task)
        {
            _dbcontext.Remove(task);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<Task> GetTaskByIdAsync(int id)
        {
            return await _dbcontext.Tasks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Task>> GetTasksByLessonIdAsync(int lessonId)
        {
            return await _dbcontext.Tasks.Where(x => x.LessonId == lessonId)
                .OrderBy(x => x.Number).ToListAsync();
        }

        public async Task<Task> UpdateTaskAsync(Task task)
        {
            _dbcontext.Tasks.Update(task);
            await _dbcontext.SaveChangesAsync();
            return task;
        }
    }
}


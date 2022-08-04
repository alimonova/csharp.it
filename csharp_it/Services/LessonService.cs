using System;
using csharp_it.Models;
using csharp_it.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace csharp_it.Services
{
	public class LessonService : ILessonService
	{
        private readonly Models.DbContext _dbcontext;

        public LessonService(Models.DbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = dbContext;
        }

        public async Task<Lesson> CreateLessonAsync(Lesson lesson)
        {
            await _dbcontext.Lessons.AddAsync(lesson);
            await _dbcontext.SaveChangesAsync();

            return lesson;
        }

        public async System.Threading.Tasks.Task DeleteAsync(Lesson lesson)
        {
            _dbcontext.Remove(lesson);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<Lesson> GetLessonByIdAsync(int id)
        {
            return await _dbcontext.Lessons.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Lesson>> GetLessonsByChapterIdAsync(int chapterId)
        {
            return await _dbcontext.Lessons.Where(x=>x.ChapterId == chapterId)
                .OrderBy(x => x.Number).ToListAsync();
        }
        public async Task<Lesson> UpdateLessonAsync(Lesson lesson)
        {
            _dbcontext.Lessons.Update(lesson);
            await _dbcontext.SaveChangesAsync();
            return lesson;
        }
    }
}


using System;
using csharp_it.Models;
using csharp_it.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace csharp_it.Services
{
	public class ChapterService : IChapterService
	{
        private readonly Models.DbContext _dbcontext;

        public ChapterService(Models.DbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = dbContext;
        }

        public async Task<Chapter> CreateChapterAsync(Chapter chapter)
        {
            await _dbcontext.Chapters.AddAsync(chapter);
            await _dbcontext.SaveChangesAsync();

            return chapter;
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var chapter = await GetChapterByIdAsync(id);
            _dbcontext.Remove(chapter);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<Chapter> GetChapterByIdAsync(int id)
        {
            return await _dbcontext.Chapters.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Chapter>> GetChaptersByCourseIdAsync(int courseId)
        {
            return await _dbcontext.Chapters.Where(x=>x.CourseId == courseId)
                .OrderBy(x => x.Number).ToListAsync();
        }

        public async Task<Chapter> UpdateChapterAsync(Chapter chapter)
        {
            _dbcontext.Chapters.Update(chapter);
            await _dbcontext.SaveChangesAsync();
            return chapter;
        }
    }
}


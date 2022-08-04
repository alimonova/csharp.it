using System;
using csharp_it.Models;
using csharp_it.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace csharp_it.Services
{
	public class PracticalExampleService : IPracticalExampleService
    {
        private readonly Models.DbContext _dbcontext;

        public PracticalExampleService(Models.DbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = dbContext;
        }

        public async Task<PracticalExample> CreatePracticalExampleAsync(PracticalExample example)
        {
            await _dbcontext.PracticalExamples.AddAsync(example);
            await _dbcontext.SaveChangesAsync();

            return example;
        }

        public async System.Threading.Tasks.Task DeleteAsync(PracticalExample example)
        {
            _dbcontext.Remove(example);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<PracticalExample> GetPracticalExampleByIdAsync(int id)
        {
            return await _dbcontext.PracticalExamples.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<PracticalExample>> GetPracticalExamplesByLessonIdAsync(int lessonId)
        {
            return await _dbcontext.PracticalExamples.Where(x => x.LessonId == lessonId)
                .OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<PracticalExample> UpdatePracticalExampleAsync(PracticalExample example)
        {
            _dbcontext.PracticalExamples.Update(example);
            await _dbcontext.SaveChangesAsync();
            return example;
        }
    }
}

using System;
using csharp_it.Models;
using csharp_it.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace csharp_it.Services
{
	public class QuestionService : IQuestionService
    {
        private readonly Models.DbContext _dbcontext;

        public QuestionService(Models.DbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = dbContext;
        }

        public async Task<Question> CreateQuestionAsync(Question question)
        {
            await _dbcontext.Questions.AddAsync(question);
            await _dbcontext.SaveChangesAsync();

            return question;
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var question = await GetQuestionByIdAsync(id);
            _dbcontext.Remove(question);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<Question> GetQuestionByIdAsync(int id)
        {
            return await _dbcontext.Questions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Question>> GetQuestionsByLessonIdAsync(int lessonId)
        {
            return await _dbcontext.Questions.Where(x => x.LessonId == lessonId)
                .OrderBy(x => x.Number).ToListAsync();
        }

        public async Task<Question> UpdateQuestionAsync(Question question)
        {
            _dbcontext.Questions.Update(question);
            await _dbcontext.SaveChangesAsync();
            return question;
        }
    }
}


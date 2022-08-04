using System;
using csharp_it.Models;
using csharp_it.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace csharp_it.Services
{
	public class AnswerService : IAnswerService
	{
        private readonly Models.DbContext _dbcontext;

        public AnswerService(Models.DbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = dbContext;
        }

        public async Task<Answer> CreateAnswerAsync(Answer answer)
        {
            await _dbcontext.Answers.AddAsync(answer);
            await _dbcontext.SaveChangesAsync();

            return answer;
        }

        public async System.Threading.Tasks.Task DeleteAsync(Answer answer)
        {
            _dbcontext.Remove(answer);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<Answer> GetAnswerByIdAsync(int id)
        {
            return await _dbcontext.Answers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(int questionId)
        {
            return await _dbcontext.Answers.Where(x => x.QuestionId == questionId)
                .OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<Answer> UpdateAnswerAsync(Answer answer)
        {
            _dbcontext.Answers.Update(answer);
            await _dbcontext.SaveChangesAsync();
            return answer;
        }

        public async Task<double> CheckTest(List<int> answers)
        {
            int rightAnswers = 0;
            var answer = new Answer();
            foreach (var answerId in answers)
            {
                answer = await GetAnswerByIdAsync(answerId);
                if (answer == null)
                {
                    return -1;
                }

                if (answer.RightAnswer)
                {
                    rightAnswers++;
                }
            }

            var questionsCount = answer.Question.Lesson.Questions.Count();

            return (double)(rightAnswers / questionsCount) * 100;
        }
    }
}


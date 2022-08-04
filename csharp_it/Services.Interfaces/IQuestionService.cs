using System;
using csharp_it.Models;

namespace csharp_it.Services.Interfaces
{
	public interface IQuestionService
	{
        Task<Question> CreateQuestionAsync(Question question);
        System.Threading.Tasks.Task DeleteAsync(Question question);
        Task<Question> GetQuestionByIdAsync(int id);
        Task<IEnumerable<Question>> GetQuestionsByLessonIdAsync(int lessonId);
        Task<Question> UpdateQuestionAsync(Question question);
    }
}


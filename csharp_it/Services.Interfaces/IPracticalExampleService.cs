using System;
using csharp_it.Models;

namespace csharp_it.Services.Interfaces
{
	public interface IPracticalExampleService
	{
        Task<PracticalExample> CreatePracticalExampleAsync(PracticalExample example);
        System.Threading.Tasks.Task DeleteAsync(int id);
        Task<PracticalExample> GetPracticalExampleByIdAsync(int id);
        Task<IEnumerable<PracticalExample>> GetPracticalExamplesByLessonIdAsync(int lessonId);
        Task<PracticalExample> UpdatePracticalExampleAsync(PracticalExample example);
    }
}


using System;
using csharp_it.Models;

namespace csharp_it.Services.Interfaces
{
	public interface IChapterService
	{
        Task<Chapter> CreateChapterAsync(Chapter chapter);
        System.Threading.Tasks.Task DeleteAsync(int id);
        Task<Chapter> GetChapterByIdAsync(int id);
        Task<IEnumerable<Chapter>> GetChaptersByCourseIdAsync(int courseId);
        Task<Chapter> UpdateChapterAsync(Chapter chapter);
    }
}


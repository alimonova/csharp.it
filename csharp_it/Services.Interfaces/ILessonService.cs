using System;
using csharp_it.Models;

namespace csharp_it.Services.Interfaces
{
	public interface ILessonService
	{
        Task<Lesson> CreateLessonAsync(Lesson lesson);
        System.Threading.Tasks.Task DeleteAsync(int id);
        Task<Lesson> GetLessonByIdAsync(int id);
        Task<IEnumerable<Lesson>> GetLessonsByChapterIdAsync(int chapterId);
        Task<Lesson> UpdateLessonAsync(Lesson lesson);
    }
}


using System;
using csharp_it.Models;

namespace csharp_it.Services.Interfaces
{
	public interface IUsefulResourceService
	{
        Task<UsefulResource> CreateUsefulResourceAsync(UsefulResource resource);
        System.Threading.Tasks.Task DeleteAsync(UsefulResource resource);
        Task<UsefulResource> GetUsefulResourceByIdAsync(int id);
        Task<IEnumerable<UsefulResource>> GetUsefulResourcesByLessonIdAsync(int lessonId);
        Task<UsefulResource> UpdateUsefulResourceAsync(UsefulResource resource);
    }
}


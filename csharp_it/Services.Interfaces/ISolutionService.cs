using System;
using csharp_it.Models;

namespace csharp_it.Services.Interfaces
{
	public interface ISolutionService
	{
        Task<Solution> CreateSolutionAsync(Solution solution);
        Task<Solution> GetSolutionByIdAsync(Guid id);
        Task<IEnumerable<Solution>> GetSolutionsByTaskIdAsync(int taskId);
        Task<Solution> UpdateSolutionAsync(Solution solution);
        Task<IEnumerable<Solution>> GetSolutionsByUserIdAsync(Guid userId);
    }
}


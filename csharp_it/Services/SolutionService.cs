using System;
using csharp_it.Models;
using csharp_it.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace csharp_it.Services
{
	public class SolutionService : ISolutionService
    {
        private readonly Models.DbContext _dbcontext;

        public SolutionService(Models.DbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<Solution> CreateSolutionAsync(Solution solution)
        {
            solution.AttemptNumber = 1;
            await _dbcontext.Solutions.AddAsync(solution);
            await _dbcontext.SaveChangesAsync();

            return solution;
        }

        public async Task<Solution> GetSolutionByIdAsync(Guid id)
        {
            return await _dbcontext.Solutions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Solution>> GetSolutionsByTaskIdAsync(int taskId)
        {
            return await _dbcontext.Solutions.Where(x => x.TaskId == taskId)
                .OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<IEnumerable<Solution>> GetSolutionsByUserIdAsync(Guid userId)
        {
            return await _dbcontext.Solutions.Where(x => x.UserId == userId)
                .OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<Solution> UpdateSolutionAsync(Solution solution)
        {
            _dbcontext.Solutions.Update(solution);
            await _dbcontext.SaveChangesAsync();
            return solution;
        }
    }
}


using System;
using csharp_it.Models;
using csharp_it.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace csharp_it.Services
{
    public class UsefulResourceService : IUsefulResourceService
    {
        private readonly Models.DbContext _dbcontext;

        public UsefulResourceService(Models.DbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = dbContext;
        }

        public async Task<UsefulResource> CreateUsefulResourceAsync(UsefulResource resource)
        {
            await _dbcontext.UsefulResources.AddAsync(resource);
            await _dbcontext.SaveChangesAsync();

            return resource;
        }

        public async System.Threading.Tasks.Task DeleteAsync(UsefulResource resource)
        {
            _dbcontext.Remove(resource);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<UsefulResource> GetUsefulResourceByIdAsync(int id)
        {
            return await _dbcontext.UsefulResources.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<UsefulResource>> GetUsefulResourcesByLessonIdAsync(int lessonId)
        {
            return await _dbcontext.UsefulResources.Where(x => x.LessonId == lessonId)
                .OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<UsefulResource> UpdateUsefulResourceAsync(UsefulResource resource)
        {
            _dbcontext.UsefulResources.Update(resource);
            await _dbcontext.SaveChangesAsync();
            return resource;
        }
    }
}


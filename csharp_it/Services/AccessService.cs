using System;
using csharp_it.Models;
using csharp_it.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace csharp_it.Services
{
	public class AccessService : IAccessService
    {
        private readonly Models.DbContext _dbcontext;

        public AccessService(Models.DbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = dbContext;
        }

        public async Task<Access> CreateAccessAsync(Access access)
        {
            await _dbcontext.Accesses.AddAsync(access);
            await _dbcontext.SaveChangesAsync();

            return access;
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var access = await GetAccessByIdAsync(id);
            _dbcontext.Remove(access);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Access>> GetAccesses()
        {
            return await _dbcontext.Accesses.ToListAsync();
        }

        public async Task<Access> GetAccessByIdAsync(int id)
        {
            return await _dbcontext.Accesses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Access> UpdateAccessAsync(Access access)
        {
            _dbcontext.Accesses.Update(access);
            await _dbcontext.SaveChangesAsync();
            return access;
        }
    }
}


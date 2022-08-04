using System;
using csharp_it.Models;
using csharp_it.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace csharp_it.Services
{
	public class TarifService : ITarifService
    {
        private readonly Models.DbContext _dbcontext;

        public TarifService(Models.DbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = dbContext;
        }

        public async Task<TarifAccess> AddAccessToTarifAsync(Guid tarifId, int accessId)
        {
            var tarifAccess = new TarifAccess { TarifId = tarifId, AccessId = accessId };
            await _dbcontext.TarifAccesses.AddAsync(tarifAccess);
            await _dbcontext.SaveChangesAsync();

            return tarifAccess;
        }

        public async Task<Tarif> CreateTarifAsync(Tarif tarif)
        {
            await _dbcontext.Tarifs.AddAsync(tarif);
            await _dbcontext.SaveChangesAsync();

            return tarif;
        }

        public async System.Threading.Tasks.Task DeleteAsync(Tarif tarif)
        {
            _dbcontext.Remove(tarif);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<Tarif> GetTarifByIdAsync(Guid id)
        {
            return await _dbcontext.Tarifs.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TarifAccess> GetTarifAccessAsync(Guid tarifId, int accessId)
        {
            return await _dbcontext.TarifAccesses.FirstOrDefaultAsync(
                x => x.TarifId == tarifId && x.AccessId == accessId);
        }

        public async Task<IEnumerable<Tarif>> GetTarifsByCourseIdAsync(int courseId)
        {
            return await _dbcontext.Tarifs.Where(x => x.CourseId == courseId)
                .OrderBy(x => x.Price).ToListAsync();
        }

        public async System.Threading.Tasks.Task RemoveAccessFromTarifAsync(TarifAccess tarifAccess)
        {
            _dbcontext.TarifAccesses.Remove(tarifAccess);
            await _dbcontext.SaveChangesAsync();
        }
    }
}


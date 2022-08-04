using System;
using csharp_it.Models;

namespace csharp_it.Services.Interfaces
{
	public interface ITarifService
	{
        Task<Tarif> CreateTarifAsync(Tarif tarif);
        System.Threading.Tasks.Task DeleteAsync(Tarif tarif);
        Task<Tarif> GetTarifByIdAsync(Guid id);
        Task<IEnumerable<Tarif>> GetTarifsByCourseIdAsync(int courseId);
        Task<TarifAccess> AddAccessToTarifAsync(Guid tarifId, int accessId);
        Task<TarifAccess> GetTarifAccessAsync(Guid tarifId, int accessId)
        System.Threading.Tasks.Task RemoveAccessFromTarifAsync(TarifAccess tarifAccess);
    }
}


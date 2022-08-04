using System;
using csharp_it.Models;

namespace csharp_it.Services.Interfaces
{
	public interface IAccessService
	{
        Task<Access> CreateAccessAsync(Access access);
        System.Threading.Tasks.Task DeleteAsync(Access access);
        Task<Access> GetAccessByIdAsync(int id);
        Task<Access> UpdateAccessAsync(Access access);
        Task<IEnumerable<Access>> GetAccesses();
    }
}


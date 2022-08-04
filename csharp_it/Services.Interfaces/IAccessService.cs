using System;
using csharp_it.Models;

namespace csharp_it.Services.Interfaces
{
	public interface IAccessService
	{
        Task<Access> CreateAccessAsync(Access access);
        System.Threading.Tasks.Task DeleteAsync(int id);
        Task<Access> GetAccessByIdAsync(int id);
        Task<Access> UpdateAccessAsync(Access access);
    }
}


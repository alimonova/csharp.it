using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using csharp.it.Models;

namespace csharp.it.Services.Interfaces
{
	public interface ICourseService
	{
        Task<Course> CreateCourseAsync(Course course);
        System.Threading.Tasks.Task DeleteAsync(int id);
        Task<Course> GetCourseByIdAsync(int id);
        Task<IEnumerable<Course>> GetCoursesAsync();
        Task<IEnumerable<Course>> GetCoursesByStudentIdAsync(Guid studentId);
        Task<IEnumerable<Course>> GetCoursesByTeacherIdAsync(Guid teacherId);
        Task<Course> UpdateCourseAsync(Course course);
    }
}


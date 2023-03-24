using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using csharp_it.Dto;
using csharp_it.Models;

namespace csharp_it.Services.Interfaces
{
	public interface ICourseService
	{
        Task<Course> CreateCourseAsync(Teacher teacher, NewCourseDto model);
        System.Threading.Tasks.Task DeleteAsync(Course course);
        Task<Course> GetCourseByIdAsync(int id);
        Task<IEnumerable<Course>> GetCoursesAsync();
        Task<IEnumerable<UserCourse>> GetCoursesByStudentIdAsync(Guid studentId);
        Task<IEnumerable<Course>> GetCoursesByTeacherIdAsync(int teacherId);
        Task<Course> UpdateCourseAsync(Course course, UpdateCourseDto model);
        Task<UserCourse> AddUserToCourse(Guid userId, Guid tarifId);
        Task<Course> UpdateCourseLessonNumAsync(int lessonNum, Tarif tarif, Guid userId);
        Task<Teacher> AddTeacherAsync(Guid userId, string description);
    }
}


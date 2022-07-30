using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using csharp_it.Models;
using csharp_it.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace csharp_it.Services
{
    public class CourseService : ICourseService
    {
        private readonly Models.DbContext _dbcontext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CourseService(Models.DbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Course> CreateCourseAsync(Course course)
        {
            await _dbcontext.Courses.AddAsync(course);
            await _dbcontext.SaveChangesAsync();

            return course;
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var course = await GetCourseByIdAsync(id);
            _dbcontext.Remove(course);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _dbcontext.Courses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Course>> GetCoursesAsync()
        {
            return await _dbcontext.Courses.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesByStudentIdAsync(Guid studentId)
        {
            var studentCourses = await _dbcontext.UserCourses.Where(x => x.UserId == studentId).ToListAsync();
            var courses = new List<Course>();

            foreach (var x in studentCourses)
            {
                courses.Add(x.Course);
            }

            return courses;
        }

        public async Task<IEnumerable<Course>> GetCoursesByTeacherIdAsync(Guid teacherId)
        {
            return await _dbcontext.Courses.Where(x => x.AuthorId == teacherId).ToListAsync();
        }

        public async Task<Course> UpdateCourseAsync(Course course)
        {
            _dbcontext.Courses.Update(course);
            await _dbcontext.SaveChangesAsync();
            return course;
        }

        public async Task<UserCourse> AddUserToCourse(Guid userId, int courseId)
        {
            var userCourse = new UserCourse { CourseId = courseId, UserId = userId };
            await _dbcontext.UserCourses.AddAsync(userCourse);
            await _dbcontext.SaveChangesAsync();

            return userCourse;
        }
    }
}


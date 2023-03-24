using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using csharp_it.Dto;
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

        public CourseService(Models.DbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<Course> CreateCourseAsync(Teacher teacher, NewCourseDto model)
        {
            var course = new Course
            {
                Name = model.Name,
                Description = model.Description,
                TeacherId = teacher.Id,
                Created = DateTime.Now
            };

            await _dbcontext.Courses.AddAsync(course);
            await _dbcontext.SaveChangesAsync();

            return course;
        }

        public async System.Threading.Tasks.Task DeleteAsync(Course course)
        {
            _dbcontext.Remove(course);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _dbcontext.Courses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Course>> GetCoursesAsync()
        {
            var courses = await _dbcontext.Courses.OrderBy(x => x.Name).ToListAsync();
            return courses;
        }

        public async Task<IEnumerable<UserCourse>> GetCoursesByStudentIdAsync(Guid studentId)
        {
            var userCourses = await _dbcontext.UserCourses.Where(x => x.UserId == studentId).ToListAsync();

            return userCourses;
        }

        public async Task<IEnumerable<Course>> GetCoursesByTeacherIdAsync(int teacherId)
        {
            return await _dbcontext.Courses.Where(x => x.TeacherId == teacherId).ToListAsync();
        }

        public async Task<Course> UpdateCourseAsync(Course course, UpdateCourseDto model)
        {
            if (model.Name != "")
            {
                course.Name = model.Name;
            }

            if (model.Description != "")
            {
                course.Description = model.Description;
            }

            if (model.AboutAuthor != "")
            {
                _dbcontext.Teachers.First(x => x.Id == course.TeacherId).Description = model.AboutAuthor;
            }

            await _dbcontext.SaveChangesAsync();
            return course;
        }

        public async Task<Course> UpdateCourseLessonNumAsync(int lessonNum, Tarif tarif, Guid userId)
        {
            var course = tarif.Course;
            var userCourse = await _dbcontext.UserCourses.FirstOrDefaultAsync(x =>
                 x.UserId == userId && x.TarifId == tarif.Id);

            if (userCourse == null)
            {
                return null;
            }

            userCourse.CurrentLessonNumber = lessonNum;
            await _dbcontext.SaveChangesAsync();
            return course;
        }

        public async Task<UserCourse> AddUserToCourse(Guid userId, Guid tarifId)
        {
            var userCourse = new UserCourse { TarifId = tarifId, UserId = userId };
            await _dbcontext.UserCourses.AddAsync(userCourse);
            await _dbcontext.SaveChangesAsync();

            return userCourse;
        }

        public async Task<Teacher> AddTeacherAsync(Guid userId, string description)
        {
            var teacher = await _dbcontext.Teachers.FirstOrDefaultAsync(x=>x.UserId == userId);
            if (teacher == null)
            {
                await _dbcontext.Teachers.AddAsync(
                    new Teacher { Description = description, UserId = userId });
            }
            else if (description != "")
            {
                teacher.Description = description;
            }

            await _dbcontext.SaveChangesAsync();
            return teacher;
        }
    }
}


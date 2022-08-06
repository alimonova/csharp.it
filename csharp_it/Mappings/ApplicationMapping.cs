using System;
using csharp_it.Dto;
using csharp_it.Models;
using Task = csharp_it.Models.Task;

namespace csharp_it.Mappings
{
    public class ApplicationMapping : AutoMapper.Profile
    {
        public ApplicationMapping()
        {
            CreateMap<Access, AccessDto>().ReverseMap();
            CreateMap<Answer, AnswerDto>().ReverseMap();
            CreateMap<Answer, AnswerRightDto>().ReverseMap();
            CreateMap<Chapter, ChapterDto>().ReverseMap();
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Lesson, LessonDto>().ReverseMap();
            CreateMap<PracticalExample, PracticalExampleDto>().ReverseMap();
            CreateMap<Question, QuestionDto>().ReverseMap();
            CreateMap<Question, QuestionNoExplanationDto>().ReverseMap();
            CreateMap<Tarif, TarifDto>().ReverseMap();
            CreateMap<Task, TaskDto>().ReverseMap();
            CreateMap<UsefulResource, UsefulResourceDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserCourse, UserCourseDto>().ReverseMap();
            CreateMap<UserTask, UserTaskDto>().ReverseMap();
            CreateMap<Solution, SolutionDto>().ReverseMap();
            CreateMap<Solution, SolutionCreationDto>().ReverseMap();
        }
    }
}


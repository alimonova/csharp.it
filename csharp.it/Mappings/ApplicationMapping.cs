using System;
using csharp.it.Dto;
using csharp.it.Models;

namespace csharp.it.Mappings
{
    public class ApplicationMapping : AutoMapper.Profile
    {
        public ApplicationMapping()
        {
            CreateMap<Answer, AnswerDto>().ReverseMap();
            CreateMap<Chapter, ChapterDto>().ReverseMap();
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Group, GroupDto>().ReverseMap();
            CreateMap<Lesson, LessonDto>().ReverseMap();
            CreateMap<PracticalExample, PracticalExampleDto>().ReverseMap();
            CreateMap<Question, QuestionDto>().ReverseMap();
            CreateMap<Tarif, TarifDto>().ReverseMap();
            CreateMap<Task, TaskDto>().ReverseMap();
            CreateMap<UsefulResource, UsefulResourceDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserGroup, UserGroupDto>().ReverseMap();
            CreateMap<UserTask, UserTaskDto>().ReverseMap();
        }
    }
}


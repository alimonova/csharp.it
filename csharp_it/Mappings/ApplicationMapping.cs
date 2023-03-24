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
            CreateMap<TarifAccess, TarifAccessDto>().ReverseMap();
            CreateMap<Tarif, TarifDto>().ReverseMap();
            CreateMap<Tarif, TarifDetailsDto>().ReverseMap();
            CreateMap<Task, TaskDto>().ReverseMap();
            CreateMap<UsefulResource, UsefulResourceDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserCourse, UserCourseDto>().ReverseMap();
            CreateMap<UserTask, UserTaskDto>().ReverseMap();
            CreateMap<Solution, SolutionDto>().ReverseMap();
            CreateMap<Solution, SolutionCreationDto>().ReverseMap();
            CreateMap<PracticalExample, PracticalExampleDto>().ReverseMap();
            CreateMap<Question, EmptyQuestionDto>().ReverseMap();
            CreateMap<Question, QuestionDto>().ReverseMap();
            CreateMap<Question, QuestionDetailsDto>().ReverseMap();
            CreateMap<Question, QuestionNoExplanationDto>().ReverseMap();
            CreateMap<Lesson, LessonDto>().ReverseMap();
            CreateMap<Lesson, LessonDetailsDto>().ReverseMap();
            CreateMap<Chapter, ChapterDto>().ReverseMap();
            CreateMap<Chapter, ChapterDetailsDto>().ReverseMap();
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Course, CourseWithTarifsDto>().ReverseMap();
            CreateMap<Course, CourseDetailsDto>().ReverseMap();
        }
    }
}


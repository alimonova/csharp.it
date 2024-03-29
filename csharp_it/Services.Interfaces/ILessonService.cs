﻿using System;
using csharp_it.Models;

namespace csharp_it.Services.Interfaces
{
	public interface ILessonService
	{
        Task<Lesson> CreateLessonAsync(Lesson lesson);
        System.Threading.Tasks.Task DeleteAsync(Lesson lesson);
        Task<Lesson> GetLessonByIdAsync(int id);
        Task<IEnumerable<Lesson>> GetLessonsByChapterIdAsync(int chapterId);
        Task<Lesson> UpdateLessonAsync(Lesson lesson);
        Task<TestResult> CheckTestAsync(List<int> answers, Lesson lesson);
        List<QuestionWithAnswer> GetRightAnswersByLesson(Lesson lesson);
    }
}


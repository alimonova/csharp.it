using System;
using csharp_it.Models;
using csharp_it.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace csharp_it.Services
{
	public class LessonService : ILessonService
	{
        private readonly Models.DbContext _dbcontext;

        public LessonService(Models.DbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = dbContext;
        }

        public async Task<Lesson> CreateLessonAsync(Lesson lesson)
        {
            var chapter = await _dbcontext.Chapters.FirstOrDefaultAsync(x =>
                x.Id == lesson.ChapterId);

            if (chapter == null)
            {
                return null;
            }

            chapter.Course.Duration++;
            await _dbcontext.Lessons.AddAsync(lesson);
            await _dbcontext.SaveChangesAsync();

            return lesson;
        }

        public async System.Threading.Tasks.Task DeleteAsync(Lesson lesson)
        {
            lesson.Chapter.Course.Duration--;
            _dbcontext.Remove(lesson);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<Lesson> GetLessonByIdAsync(int id)
        {
            return await _dbcontext.Lessons.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Lesson>> GetLessonsByChapterIdAsync(int chapterId)
        {
            return await _dbcontext.Lessons.Where(x=>x.ChapterId == chapterId)
                .OrderBy(x => x.Number).ToListAsync();
        }
        public async Task<Lesson> UpdateLessonAsync(Lesson lesson)
        {
            _dbcontext.Lessons.Update(lesson);
            await _dbcontext.SaveChangesAsync();
            return lesson;
        }

        public async Task<TestResult> CheckTestAsync(List<int> answers, Lesson lesson)
        {
            TestResult result = new TestResult();

            foreach (var question in lesson.Questions)
            {
                if (!question.Multiple)
                {
                    var rightAnswer = await _dbcontext.Answers.FirstAsync(x=>
                    x.QuestionId == question.Id && x.RightAnswer == true);
                    if (answers.Contains(rightAnswer.Id))
                    {
                        result.Mark++;
                    }
                    else
                    {
                        result.FalseQuestions.Add(question.Id);
                    }
                    continue;
                }

                var questionAnswers = _dbcontext.Answers.Where(x =>
                    x.QuestionId == question.Id);

                var right = true;
                foreach (var answer in questionAnswers)
                {
                    if ((answer.RightAnswer == false && answers.Contains(answer.Id)) ||
                        (answer.RightAnswer == true && !answers.Contains(answer.Id)))
                    {
                        right = false;
                        result.FalseQuestions.Add(question.Id);
                        break;
                    }
                }
                if (right)
                    result.Mark++;
            }

            return result;
        }

        public List<QuestionWithAnswer> GetRightAnswersByLesson(Lesson lesson)
        {
            var questions = new List<QuestionWithAnswer>();
            lesson.Questions.ForEach(q =>
            {
                var answers = q.Answers.Where(a => a.RightAnswer == true)
                    .ToList().Select(x => x.Id).ToList();
                questions.Add(new QuestionWithAnswer { Id = q.Id, Answers = answers });
            });

            return questions;
        }
    }
}


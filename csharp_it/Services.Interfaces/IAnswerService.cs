﻿using System;
using csharp_it.Models;

namespace csharp_it.Services.Interfaces
{
	public interface IAnswerService
	{
        Task<Answer> CreateAnswerAsync(Answer answer);
        System.Threading.Tasks.Task DeleteAsync(Answer answer);
        Task<Answer> GetAnswerByIdAsync(int id);
        Task<IEnumerable<Answer>> GetAnswersByQuestionIdAsync(int qusestionId);
        Task<Answer> UpdateAnswerAsync(Answer answer);
    }
}


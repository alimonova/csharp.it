using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using csharp_it.Models;
using Microsoft.AspNetCore.Identity;

namespace csharp_it.Services.Interfaces
{
	public interface IAccountService
	{
		Task<IdentityResult> Register(UserRegistration model);
		Task<JwtSecurityToken> Authenticate(UserAuthorization model);
		Task<string> SetEmailConfirmationCode(string email);
		string FormLetterEmailConfirmation(string token, string email);
		Task<User> GetCurrentUserAsync();
		Task<IEnumerable<User>> GetUsersOfRole(string role);
		Task<IEnumerable<User>> GetStudentsOfCourse(int courseId);
		Task<IEnumerable<string>> GetCurrentUserRoles();
		Task<IEnumerable<string>> GetRolesByUserEmail(string email);
		Task<double> UpdateAccountWallet(double sum);
		Transaction[] GetTransactions();
		Task<double> BuyCourse(Guid tarifId, bool month);
		Task<bool> CheckAccessToCourse(int courseId, string accessName);
		Task<IdentityResult> ConfirmEmail(string email, string token);
		Task<bool> CheckEmailConfirmationAsync(string email);
		Task<string> ForgotPassword(string email);
		string FormLetterPasswordRecovery(string link);
		Task<IdentityResult> ResetPassword(ResetPassword model);
		Task<User> GetUserByEmailAsync(string email);
	}
}


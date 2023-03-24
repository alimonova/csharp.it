using System;
namespace csharp_it.Services.Interfaces
{
	public interface IEmailSender
	{
		Task SendEmailAsync(string email, string subject, string htmlMessage);
	}
}


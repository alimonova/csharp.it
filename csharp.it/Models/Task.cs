using System;
using System.Collections.Generic;

namespace csharp.it.Models
{
	public class Task
	{
		public int Id { get; set; }
		public string Text { get; set; }
		public int Number { get; set; }
		public int LessonId { get; set; }
		public Lesson Lesson { get; set; }
		public string Example { get; set; }
		public string Tips { get; set; }

		public List<UserTask> UserTasks { get; set; }

		public Task()
		{
			UserTasks = new List<UserTask>();
		}
	}
}


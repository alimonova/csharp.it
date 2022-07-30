namespace csharp_it.Models
{
	public class PracticalExample
	{
		public int Id { get; set; }
		public string? Description { get; set; }
		public string? Explanation { get; set; }
		public string? Code { get; set; }
		public string Name { get; set; }
		public int LessonId { get; set; }
		public Lesson Lesson { get; set; }
	}
}


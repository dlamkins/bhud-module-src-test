using System;

namespace CinemaModule.Models
{
	public class SavedStream
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public StreamSourceType SourceType { get; set; }

		public string Value { get; set; }

		public DateTime CreatedAt { get; set; }

		public SavedStream()
		{
			Id = Guid.NewGuid().ToString("N").Substring(0, 8);
			CreatedAt = DateTime.UtcNow;
			SourceType = StreamSourceType.Url;
		}

		public SavedStream(string name, StreamSourceType sourceType, string value)
			: this()
		{
			Name = name;
			SourceType = sourceType;
			Value = value;
		}
	}
}

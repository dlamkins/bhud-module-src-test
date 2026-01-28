namespace LTMessages
{
	public class MessageEntry
	{
		private const int MaxTitleLength = 16;

		private string _title;

		private string _message;

		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				_title = TruncateString(value, 16);
			}
		}

		public string Message
		{
			get
			{
				return _message;
			}
			set
			{
				_message = value ?? string.Empty;
			}
		}

		public MessageEntry(string title, string message)
		{
			Title = title;
			Message = message;
		}

		private static string TruncateString(string value, int maxLength)
		{
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}
			if (value.Length > maxLength)
			{
				return value.Substring(0, maxLength);
			}
			return value;
		}

		public override string ToString()
		{
			return Title + ": " + Message;
		}
	}
}

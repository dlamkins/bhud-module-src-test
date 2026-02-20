using System;
using Microsoft.Xna.Framework;

namespace CinemaModule.Models
{
	public class TwitchChatMessage
	{
		public string Username { get; set; }

		public string DisplayName { get; set; }

		public string Message { get; set; }

		public Color UserColor { get; set; }

		public DateTime Timestamp { get; set; }

		public bool IsAction { get; set; }

		public TwitchChatMessage()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			Timestamp = DateTime.UtcNow;
			UserColor = Color.get_White();
		}

		public TwitchChatMessage(string username, string displayName, string message, Color userColor, bool isAction = false)
			: this()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			Username = username;
			DisplayName = displayName;
			Message = message;
			UserColor = userColor;
			IsAction = isAction;
		}
	}
}

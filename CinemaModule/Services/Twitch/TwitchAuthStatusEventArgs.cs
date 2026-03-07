using System;

namespace CinemaModule.Services.Twitch
{
	public class TwitchAuthStatusEventArgs : EventArgs
	{
		public TwitchAuthStatus Status { get; }

		public string Message { get; }

		public string Username { get; }

		public string UserId { get; }

		public string AccessToken { get; }

		public string RefreshToken { get; }

		public TwitchAuthStatusEventArgs(TwitchAuthStatus status, string message, string username = null, string userId = null, string accessToken = null, string refreshToken = null)
		{
			Status = status;
			Message = message;
			Username = username;
			UserId = userId;
			AccessToken = accessToken;
			RefreshToken = refreshToken;
		}
	}
}

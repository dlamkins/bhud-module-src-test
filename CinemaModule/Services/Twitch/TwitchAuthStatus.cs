namespace CinemaModule.Services.Twitch
{
	public enum TwitchAuthStatus
	{
		NotAuthenticated,
		WaitingForUser,
		Authenticated,
		Failed,
		Cancelled
	}
}

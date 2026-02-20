namespace CinemaModule.Services
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

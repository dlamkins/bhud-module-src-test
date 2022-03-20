namespace BhModule.Community.Pathing.Behavior
{
	public enum StandardPathableBehavior
	{
		AlwaysVisible = 0,
		ReappearOnMapChange = 1,
		ReappearOnDailyReset = 2,
		OnlyVisibleBeforeActivation = 3,
		ReappearAfterTimer = 4,
		ReappearOnMapReset = 5,
		OncePerInstance = 6,
		OnceDailyPerCharacter = 7,
		ReappearOnWeeklyReset = 101
	}
}

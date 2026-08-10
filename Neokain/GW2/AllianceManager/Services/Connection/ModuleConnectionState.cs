namespace Neokain.GW2.AllianceManager.Services.Connection
{
	public enum ModuleConnectionState
	{
		NoKey,
		BadKeyFormat,
		Connecting,
		Connected,
		FailedTransient,
		FailedTerminal
	}
}

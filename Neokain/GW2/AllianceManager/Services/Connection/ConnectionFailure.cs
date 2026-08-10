namespace Neokain.GW2.AllianceManager.Services.Connection
{
	public readonly struct ConnectionFailure
	{
		public FailureKind Kind { get; }

		public string Reason { get; }

		public ConnectionFailure(FailureKind kind, string reason)
		{
			Kind = kind;
			Reason = reason;
		}
	}
}

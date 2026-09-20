namespace Quarry.Interfaces
{
	public interface IPathingBridge
	{
		bool IsAvailable { get; }

		bool TryGetInactive(string categoryNamespace, out bool inactive);

		bool TrySetInactive(string categoryNamespace, bool inactive);
	}
}

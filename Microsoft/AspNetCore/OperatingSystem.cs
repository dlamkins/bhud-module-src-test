namespace Microsoft.AspNetCore
{
	internal sealed class OperatingSystem
	{
		private const bool _isBrowser = false;

		public static bool IsBrowser()
		{
			return false;
		}
	}
}

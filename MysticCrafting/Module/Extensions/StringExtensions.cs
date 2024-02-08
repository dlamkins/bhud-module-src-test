namespace MysticCrafting.Module.Extensions
{
	public static class StringExtensions
	{
		public static string Truncate(this string value, int maxLength, string truncationSuffix = "…")
		{
			if (value == null || value.Length <= maxLength)
			{
				return value;
			}
			return value.Substring(0, maxLength) + truncationSuffix;
		}
	}
}

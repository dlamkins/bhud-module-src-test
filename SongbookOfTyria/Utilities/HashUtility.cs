namespace SongbookOfTyria.Utilities
{
	internal static class HashUtility
	{
		public static uint GetStableHashCode(string str)
		{
			uint hash = 2166136261u;
			foreach (char c in str)
			{
				hash = (hash ^ c) * 16777619;
			}
			return hash;
		}
	}
}

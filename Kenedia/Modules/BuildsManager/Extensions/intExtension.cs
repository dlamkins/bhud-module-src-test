namespace Kenedia.Modules.BuildsManager.Extensions
{
	public static class intExtension
	{
		public static int Scale(this int i, double factor)
		{
			return (int)((double)i * factor);
		}
	}
}

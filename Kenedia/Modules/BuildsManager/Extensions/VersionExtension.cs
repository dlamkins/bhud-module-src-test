using SemVer;

namespace Kenedia.Modules.BuildsManager.Extensions
{
	public static class VersionExtension
	{
		public static Version Increment(this Version version)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			//IL_001f: Expected O, but got Unknown
			Version val = new Version(version.get_Major(), version.get_Minor(), version.get_Patch() + 1, (string)null, (string)null);
			version = val;
			return val;
		}
	}
}

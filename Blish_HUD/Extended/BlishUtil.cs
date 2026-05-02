using System.Linq;
using System.Reflection;

namespace Blish_HUD.Extended
{
	public class BlishUtil
	{
		public static string GetVersion()
		{
			string version = typeof(BlishHud).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
			if (!string.IsNullOrEmpty(version))
			{
				return "Blish HUD v" + version.Split('+').First();
			}
			return string.Empty;
		}
	}
}

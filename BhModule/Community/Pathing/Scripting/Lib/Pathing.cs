using System;
using Blish_HUD.Modules;
using SemVer;

namespace BhModule.Community.Pathing.Scripting.Lib
{
	public class Pathing
	{
		private readonly PathingGlobal _global;

		public string Version => ((Module)_global.ScriptEngine.Module).get_Version().Clean();

		internal Pathing(PathingGlobal global)
		{
			_global = global;
		}

		public bool IsVersionAtLeast(string version)
		{
			try
			{
				return ((Module)_global.ScriptEngine.Module).get_Version() >= new SemVer.Version(version);
			}
			catch (ArgumentException)
			{
				return false;
			}
		}
	}
}

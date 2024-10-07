using System.IO;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Kenedia.Modules.Core.Models;

namespace Kenedia.Modules.BuildsManager.Models
{
	public class Paths : PathCollection
	{
		public string TemplatesPath => base.BasePath + "\\" + ModuleName + "\\templates\\";

		public string ItemMapPath => base.ModuleDataPath + "\\itemmap\\";

		public Paths()
		{
		}

		public Paths(DirectoriesManager directoriesManager, Module module)
			: base(directoriesManager, module)
		{
			if (!Directory.Exists(TemplatesPath))
			{
				Directory.CreateDirectory(TemplatesPath);
			}
			if (!Directory.Exists(ItemMapPath))
			{
				Directory.CreateDirectory(ItemMapPath);
			}
		}
	}
}

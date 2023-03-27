using System.IO;
using Blish_HUD.Modules.Managers;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.Core.Models
{
	public class PathCollection
	{
		protected readonly string? ModuleName;

		protected readonly DirectoriesManager DirectoriesManager;

		private string? _accountName;

		public string? AccountName
		{
			get
			{
				return _accountName;
			}
			set
			{
				Common.SetProperty(ref _accountName, value, AddAccountFolder, !string.IsNullOrEmpty(value));
			}
		}

		public string BasePath { get; }

		public string ModulePath => BasePath + "\\" + ModuleName + "\\";

		public string ModuleDataPath => BasePath + "\\" + ModuleName + "\\data\\";

		public string SharedSettingsPath => BasePath + "\\shared_settings.json";

		public string? AccountPath
		{
			get
			{
				if (_accountName == null)
				{
					return null;
				}
				return ModulePath + AccountName + "\\";
			}
		}

		public PathCollection()
		{
		}

		public PathCollection(DirectoriesManager directoriesManager, string moduleName)
		{
			DirectoriesManager = directoriesManager;
			ModuleName = moduleName.Replace(' ', '_').ToLower();
			BasePath = DirectoriesManager.GetFullDirectoryPath("kenedia");
			if (!Directory.Exists(ModulePath))
			{
				Directory.CreateDirectory(ModulePath);
			}
			if (!Directory.Exists(ModuleDataPath))
			{
				Directory.CreateDirectory(ModuleDataPath);
			}
		}

		private void AddAccountFolder()
		{
			if (!Directory.Exists(AccountPath))
			{
				Directory.CreateDirectory(AccountPath);
			}
		}
	}
}

using System.IO;
using Blish_HUD.Modules.Managers;

namespace Kenedia.Modules.Core.Models
{
	public class PathCollection
	{
		private readonly string? _moduleName;

		private string? _accountName;

		private readonly DirectoriesManager _directoriesManager;

		public string? AccountName
		{
			get
			{
				return _accountName;
			}
			set
			{
				_accountName = value;
				if (!string.IsNullOrEmpty(value))
				{
					AddAccountFolder();
				}
			}
		}

		public string BasePath { get; }

		public string ModulePath => BasePath + "\\" + _moduleName + "\\";

		public string ModuleDataPath => BasePath + "\\" + _moduleName + "\\data\\";

		public string SharedSettingsPath => BasePath + "\\shared_settings.json";

		public string? AccountPath
		{
			get
			{
				if (_accountName == null)
				{
					return null;
				}
				return ModulePath + "\\" + AccountName + "\\";
			}
		}

		public PathCollection(DirectoriesManager directoriesManager, string moduleName)
		{
			_directoriesManager = directoriesManager;
			_moduleName = moduleName.Replace(' ', '_').ToLower();
			BasePath = _directoriesManager.GetFullDirectoryPath("kenedia");
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

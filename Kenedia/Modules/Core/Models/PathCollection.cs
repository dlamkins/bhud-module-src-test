using System;
using System.IO;
using System.Runtime.CompilerServices;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Kenedia.Modules.Core.Utility;

namespace Kenedia.Modules.Core.Models
{
	public class PathCollection
	{
		protected readonly string? ModuleName;

		protected readonly DirectoriesManager DirectoriesManager;

		public string? AccountName
		{
			[CompilerGenerated]
			get
			{
				return _003CAccountName_003Ek__BackingField;
			}
			set
			{
				Common.SetProperty(_003CAccountName_003Ek__BackingField, value, delegate(string v)
				{
					_003CAccountName_003Ek__BackingField = v;
				}, new Action(AddAccountFolder), !string.IsNullOrEmpty(value));
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
				if (AccountName == null)
				{
					return null;
				}
				return ModulePath + AccountName + "\\";
			}
		}

		public PathCollection()
		{
		}

		public PathCollection(DirectoriesManager directoriesManager, Module module)
		{
			DirectoriesManager = directoriesManager;
			ModuleName = module.Name.Replace(' ', '_').ToLower();
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

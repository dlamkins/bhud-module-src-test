using System.Linq;
using BhModule.Community.Pathing.MarkerPackRepo;
using Blish_HUD.Settings;
using TmfLib;

namespace BhModule.Community.Pathing
{
	internal class PackWrapper
	{
		private readonly PathingModule _module;

		private MarkerPackPkg _package;

		public Pack Pack { get; private set; }

		public MarkerPackPkg Package
		{
			get
			{
				if (_package == null)
				{
					_package = _module.MarkerPackRepo.MarkerPackages.FirstOrDefault((MarkerPackPkg p) => p.FileName.ToLowerInvariant().StartsWith(Pack.Name.ToLowerInvariant() + "."));
				}
				return _package;
			}
		}

		public SettingEntry<bool> AlwaysLoad { get; private set; }

		public bool ForceLoad { get; set; }

		public bool IsLoaded { get; set; }

		public long LoadTime { get; set; }

		public PackWrapper(PathingModule module, Pack pack, SettingEntry<bool> alwaysLoad)
		{
			_module = module;
			Pack = pack;
			AlwaysLoad = alwaysLoad;
		}
	}
}

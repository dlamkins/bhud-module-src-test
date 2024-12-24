using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using MysticCrafting.Models.Menu;
using MysticCrafting.Module.Services;

namespace MysticCrafting.Module.Repositories
{
	public class MenuRepository : IMenuRepository, IRepository
	{
		private readonly Logger Logger = Logger.GetLogger<MenuRepository>();

		private IList<MysticMenuItem> MenuItems { get; set; }

		public bool LocalOnly => false;

		public bool Loaded { get; private set; }

		public string FileName => "menu_data.json";

		public string DatabaseFileResourceName => "MysticCrafting.Module.EmbeddedResources." + FileName;

		public async Task<string> LoadAsync()
		{
			using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(DatabaseFileResourceName))
			{
				if (resourceStream == null)
				{
					Logger.Error("Could not find embedded resource " + FileName);
					return string.Empty;
				}
				MenuItems = await JsonSerializer.DeserializeAsync<List<MysticMenuItem>>(resourceStream, DataService._serializerOptions, default(CancellationToken));
			}
			Loaded = true;
			return $"{MenuItems.Count} menu items loaded";
		}

		public IList<MysticMenuItem> GetMenuItems()
		{
			return MenuItems ?? new List<MysticMenuItem>();
		}
	}
}

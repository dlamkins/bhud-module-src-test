using System.Collections.Generic;
using System.Threading.Tasks;
using Gw2Sharp.WebApi.V2.Models;
using flakysalt.CharacterKeybinds.Data;
using flakysalt.CharacterKeybinds.Services;
using flakysalt.CharacterKeybinds.Util;

namespace flakysalt.CharacterKeybinds.Model
{
	public class MigrationTabModel
	{
		private readonly Gw2ApiService _apiService;

		public CharacterKeybindsSettings Settings { get; }

		public MigrationTabModel(CharacterKeybindsSettings settings, Gw2ApiService apiService)
		{
			Settings = settings;
			_apiService = apiService;
		}

		public async Task<List<string>> MigrateKeybindings()
		{
			IEnumerable<Specialization> specializations = await _apiService.GetSpecializationsAsync();
			List<string> migrationReport;
			List<Keymap> keymaps = SaveDataMigration.MigrateToKeymaps(Settings.characterKeybinds.get_Value(), specializations, out migrationReport);
			Settings.Keymaps.set_Value(keymaps);
			return migrationReport;
		}

		public void DeleteOldData()
		{
			Settings.characterKeybinds.set_Value((List<CharacterKeybind>)null);
		}
	}
}

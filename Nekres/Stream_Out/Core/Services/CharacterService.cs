using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Extended;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Nekres.Stream_Out.Core.Services
{
	internal class CharacterService : ExportService
	{
		private const string CHARACTER_NAME = "character_name.txt";

		private const string PROFESSION_ICON = "profession_icon.png";

		private const string PROFESSION_NAME = "profession_name.txt";

		private const string COMMANDER_ICON = "commander_icon.png";

		private const string DEATHS_WEEK = "deaths_week.txt";

		private const string DEATHS_DAY = "deaths_day.txt";

		private const string COMBAT_ICON = "combat_icon.png";

		private const string COMBAT_TEXT = "combat.txt";

		private const string SKULL = "☠";

		private const string SWORDS = "⚔";

		private Bitmap _commanderIcon;

		private Bitmap _catmanderIcon;

		private Bitmap _battleIcon;

		private SettingEntry<int> _deathsWeekly;

		private SettingEntry<int> _deathsDaily;

		private SettingEntry<int> _deathsAtResetWeekly;

		private SettingEntry<int> _deathsAtResetDaily;

		private static Gw2ApiManager Gw2ApiManager => StreamOutModule.Instance?.Gw2ApiManager;

		private DirectoriesManager DirectoriesManager => StreamOutModule.Instance?.DirectoriesManager;

		private ContentsManager ContentsManager => StreamOutModule.Instance?.ContentsManager;

		private StreamOutModule.UnicodeSigning UnicodeSigning => StreamOutModule.Instance?.AddUnicodeSymbols.get_Value() ?? StreamOutModule.UnicodeSigning.Suffixed;

		private SettingEntry<bool> UseCatmanderTag => StreamOutModule.Instance.UseCatmanderTag;

		public CharacterService(SettingCollection settings)
			: base(settings)
		{
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)OnNameChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().add_SpecializationChanged((EventHandler<ValueEventArgs<int>>)OnSpecializationChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().add_IsCommanderChanged((EventHandler<ValueEventArgs<bool>>)OnIsCommanderChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().add_IsInCombatChanged((EventHandler<ValueEventArgs<bool>>)OnIsInCombatChanged);
			UseCatmanderTag.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUseCatmanderTagSettingChanged);
			OnNameChanged(null, new ValueEventArgs<string>(GameService.Gw2Mumble.get_PlayerCharacter().get_Name()));
			OnSpecializationChanged(null, new ValueEventArgs<int>(GameService.Gw2Mumble.get_PlayerCharacter().get_Specialization()));
			_deathsWeekly = settings.DefineSetting<int>(GetType().Name + "_deaths_weekly", 0, (Func<string>)null, (Func<string>)null);
			_deathsDaily = settings.DefineSetting<int>(GetType().Name + "_deaths_daily", 0, (Func<string>)null, (Func<string>)null);
			_deathsAtResetWeekly = settings.DefineSetting<int>(GetType().Name + "_deaths_weekly_reset", 0, (Func<string>)null, (Func<string>)null);
			_deathsAtResetDaily = settings.DefineSetting<int>(GetType().Name + "_deaths_daily_reset", 0, (Func<string>)null, (Func<string>)null);
		}

		public override async Task Initialize()
		{
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/deaths_week.txt", "0☠", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/deaths_day.txt", "0☠", overwrite: false);
			using Stream catmanderIconStream = ContentsManager.GetFileStream("catmander_tag_white.png");
			_catmanderIcon = new Bitmap(catmanderIconStream);
			using Stream commanderIconStream = ContentsManager.GetFileStream("commander_tag_white.png");
			_commanderIcon = new Bitmap(commanderIconStream);
			using Stream battleIconStream = ContentsManager.GetFileStream("240678.png");
			_battleIcon = new Bitmap(battleIconStream);
		}

		private async void OnIsInCombatChanged(object o, ValueEventArgs<bool> e)
		{
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/combat.txt", e.get_Value() ? "⚔" : string.Empty);
			if (!e.get_Value())
			{
				await TextureUtil.ClearImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/combat_icon.png");
			}
			else
			{
				await _battleIcon.SaveOnNetworkShare(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/combat_icon.png", ImageFormat.Png);
			}
		}

		private async void OnNameChanged(object o, ValueEventArgs<string> e)
		{
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/character_name.txt", e.get_Value() ?? string.Empty);
		}

		private async void OnSpecializationChanged(object o, ValueEventArgs<int> e)
		{
			if (e.get_Value() <= 0)
			{
				await TextureUtil.ClearImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/profession_icon.png");
				return;
			}
			Specialization specialization = await TaskUtil.RetryAsync(() => ((IBulkExpandableClient<Specialization, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Specializations()).GetAsync(e.get_Value(), default(CancellationToken))).Unwrap();
			if (specialization == null)
			{
				return;
			}
			RenderUrl? icon;
			string name;
			if (specialization.get_Elite())
			{
				icon = specialization.get_ProfessionIconBig();
				name = specialization.get_Name();
			}
			else
			{
				Profession profession = await TaskUtil.RetryAsync(() => ((IBulkAliasExpandableClient<Profession, ProfessionType>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Professions()).GetAsync(GameService.Gw2Mumble.get_PlayerCharacter().get_Profession(), default(CancellationToken))).Unwrap();
				if (profession == null)
				{
					return;
				}
				icon = profession.get_IconBig();
				name = profession.get_Name();
			}
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/profession_name.txt", name ?? string.Empty);
			RenderUrl? val = icon;
			await TextureUtil.SaveToImage(val.HasValue ? RenderUrl.op_Implicit(val.GetValueOrDefault()) : null, DirectoriesManager.GetFullDirectoryPath("stream_out") + "/profession_icon.png");
		}

		private async void OnIsCommanderChanged(object o, ValueEventArgs<bool> e)
		{
			if (!e.get_Value())
			{
				await TextureUtil.ClearImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/commander_icon.png");
			}
			else
			{
				await SaveCommanderIcon(UseCatmanderTag.get_Value());
			}
		}

		private async Task SaveCommanderIcon(bool useCatmanderIcon)
		{
			await (useCatmanderIcon ? _catmanderIcon : _commanderIcon).SaveOnNetworkShare(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/commander_icon.png", ImageFormat.Png);
		}

		private async void OnUseCatmanderTagSettingChanged(object o, ValueChangedEventArgs<bool> e)
		{
			if (GameService.Gw2Mumble.get_PlayerCharacter().get_IsCommander())
			{
				await SaveCommanderIcon(e.get_NewValue());
			}
		}

		public static async Task<int> RequestTotalDeaths()
		{
			if (!Gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)new TokenPermission[2]
			{
				(TokenPermission)1,
				(TokenPermission)3
			}))
			{
				return -1;
			}
			return ((IEnumerable<Character>)(await TaskUtil.RetryAsync(() => ((IAllExpandableClient<Character>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).AllAsync(default(CancellationToken))).Unwrap()))?.Sum((Character x) => x.get_Deaths()) ?? (-1);
		}

		protected override async Task<bool> ResetDaily()
		{
			int totalDeaths = await RequestTotalDeaths();
			if (totalDeaths < 0)
			{
				return false;
			}
			_deathsDaily.set_Value(0);
			_deathsAtResetDaily.set_Value(totalDeaths);
			return true;
		}

		protected override async Task<bool> ResetWeekly()
		{
			int totalDeaths = await RequestTotalDeaths();
			if (totalDeaths < 0)
			{
				return false;
			}
			_deathsWeekly.set_Value(0);
			_deathsAtResetWeekly.set_Value(totalDeaths);
			return true;
		}

		protected override async Task Update()
		{
			string prefixDeaths = ((UnicodeSigning == StreamOutModule.UnicodeSigning.Prefixed) ? "☠" : string.Empty);
			string suffixDeaths = ((UnicodeSigning == StreamOutModule.UnicodeSigning.Suffixed) ? "☠" : string.Empty);
			int totalDeaths = await RequestTotalDeaths();
			if (totalDeaths >= 0)
			{
				_deathsDaily.set_Value(totalDeaths - _deathsAtResetDaily.get_Value());
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/deaths_week.txt", $"{prefixDeaths}{_deathsDaily.get_Value()}{suffixDeaths}");
				_deathsWeekly.set_Value(totalDeaths - _deathsAtResetWeekly.get_Value());
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/deaths_day.txt", $"{prefixDeaths}{_deathsWeekly.get_Value()}{suffixDeaths}");
			}
		}

		public override async Task Clear()
		{
			string dir = DirectoriesManager.GetFullDirectoryPath("stream_out");
			await FileUtil.DeleteAsync(Path.Combine(dir, "deaths_day.txt"));
			await FileUtil.DeleteAsync(Path.Combine(dir, "deaths_week.txt"));
			await FileUtil.DeleteAsync(Path.Combine(dir, "character_name.txt"));
			await FileUtil.DeleteAsync(Path.Combine(dir, "profession_icon.png"));
			await FileUtil.DeleteAsync(Path.Combine(dir, "commander_icon.png"));
		}

		public override void Dispose()
		{
			_commanderIcon?.Dispose();
			_catmanderIcon?.Dispose();
			_battleIcon?.Dispose();
			GameService.Gw2Mumble.get_PlayerCharacter().remove_NameChanged((EventHandler<ValueEventArgs<string>>)OnNameChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_SpecializationChanged((EventHandler<ValueEventArgs<int>>)OnSpecializationChanged);
			UseCatmanderTag.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUseCatmanderTagSettingChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_IsCommanderChanged((EventHandler<ValueEventArgs<bool>>)OnIsCommanderChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_IsInCombatChanged((EventHandler<ValueEventArgs<bool>>)OnIsInCombatChanged);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.Models;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.Exceptions;
using Gw2Sharp.WebApi.V2;
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

		private const string SKULL = "☠";

		private Bitmap _commanderIcon;

		private Bitmap _catmanderIcon;

		private static Gw2ApiManager Gw2ApiManager => StreamOutModule.Instance?.Gw2ApiManager;

		private DirectoriesManager DirectoriesManager => StreamOutModule.Instance?.DirectoriesManager;

		private ContentsManager ContentsManager => StreamOutModule.Instance?.ContentsManager;

		private SettingEntry<int> SessionDeathsWvW => StreamOutModule.Instance?.SessionDeathsWvW;

		private SettingEntry<int> TotalDeathsAtResetWvW => StreamOutModule.Instance?.TotalDeathsAtResetWvW;

		private SettingEntry<int> SessionDeathsDaily => StreamOutModule.Instance?.SessionDeathsDaily;

		private SettingEntry<int> TotalDeathsAtResetDaily => StreamOutModule.Instance?.TotalDeathsAtResetDaily;

		private StreamOutModule.UnicodeSigning UnicodeSigning => StreamOutModule.Instance?.AddUnicodeSymbols.get_Value() ?? StreamOutModule.UnicodeSigning.Suffixed;

		private SettingEntry<bool> UseCatmanderTag => StreamOutModule.Instance.UseCatmanderTag;

		public CharacterService()
		{
			GameService.Gw2Mumble.get_PlayerCharacter().add_NameChanged((EventHandler<ValueEventArgs<string>>)OnNameChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().add_SpecializationChanged((EventHandler<ValueEventArgs<int>>)OnSpecializationChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().add_IsCommanderChanged((EventHandler<ValueEventArgs<bool>>)OnIsCommanderChanged);
			UseCatmanderTag.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUseCatmanderTagSettingChanged);
			OnNameChanged(null, new ValueEventArgs<string>(GameService.Gw2Mumble.get_PlayerCharacter().get_Name()));
			OnSpecializationChanged(null, new ValueEventArgs<int>(GameService.Gw2Mumble.get_PlayerCharacter().get_Specialization()));
		}

		public override async Task Initialize()
		{
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/deaths_week.txt", "0☠", overwrite: false);
			await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/deaths_day.txt", "0☠", overwrite: false);
			string moduleDir = DirectoriesManager.GetFullDirectoryPath("stream_out");
			ContentsManager.ExtractIcons(UseCatmanderTag.get_Value() ? "catmander_tag_white.png" : "commander_tag_white.png", Path.Combine(moduleDir, "commander_icon.png"));
			if (!GameService.Gw2Mumble.get_PlayerCharacter().get_IsCommander())
			{
				await TextureUtil.ClearImage(moduleDir + "/commander_icon.png");
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
			try
			{
				Specialization specialization = await ((IBulkExpandableClient<Specialization, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Specializations()).GetAsync(e.get_Value(), default(CancellationToken));
				RenderUrl? icon;
				string name;
				if (specialization.get_Elite())
				{
					icon = specialization.get_ProfessionIconBig();
					name = specialization.get_Name();
				}
				else
				{
					Profession profession = await ((IBulkAliasExpandableClient<Profession, ProfessionType>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Professions()).GetAsync(GameService.Gw2Mumble.get_PlayerCharacter().get_Profession(), default(CancellationToken));
					icon = profession.get_IconBig();
					name = profession.get_Name();
				}
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/profession_name.txt", name ?? string.Empty);
				RenderUrl? val = icon;
				await TextureUtil.SaveToImage(val.HasValue ? RenderUrl.op_Implicit(val.GetValueOrDefault()) : null, DirectoriesManager.GetFullDirectoryPath("stream_out") + "/profession_icon.png");
			}
			catch (UnexpectedStatusException)
			{
				StreamOutModule.Logger.Warn(StreamOutModule.Instance.WebApiDown);
			}
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
			if (useCatmanderIcon)
			{
				if (_catmanderIcon == null)
				{
					using Stream stream = ContentsManager.GetFileStream("catmander_tag_white.png");
					_catmanderIcon = new Bitmap(stream);
					await stream.FlushAsync();
				}
				await _catmanderIcon.SaveOnNetworkShare(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/commander_icon.png", ImageFormat.Png);
				return;
			}
			if (_commanderIcon == null)
			{
				using Stream stream = ContentsManager.GetFileStream("commander_tag_white.png");
				_commanderIcon = new Bitmap(stream);
				await stream.FlushAsync();
			}
			await _commanderIcon.SaveOnNetworkShare(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/commander_icon.png", ImageFormat.Png);
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
			return await ((IAllExpandableClient<Character>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()).AllAsync(default(CancellationToken)).ContinueWith((Task<IApiV2ObjectList<Character>> task) => (!task.IsFaulted) ? ((IEnumerable<Character>)task.Result).Sum((Character x) => x.get_Deaths()) : (-1));
		}

		protected override async Task ResetDaily()
		{
			SessionDeathsDaily.set_Value(0);
			SettingEntry<int> totalDeathsAtResetDaily = TotalDeathsAtResetDaily;
			totalDeathsAtResetDaily.set_Value(await RequestTotalDeaths());
		}

		protected override async Task Update()
		{
			string prefixDeaths = ((UnicodeSigning == StreamOutModule.UnicodeSigning.Prefixed) ? "☠" : string.Empty);
			string suffixDeaths = ((UnicodeSigning == StreamOutModule.UnicodeSigning.Suffixed) ? "☠" : string.Empty);
			int totalDeaths = await RequestTotalDeaths();
			if (totalDeaths >= 0)
			{
				SessionDeathsDaily.set_Value(totalDeaths - TotalDeathsAtResetDaily.get_Value());
				SessionDeathsWvW.set_Value(totalDeaths - TotalDeathsAtResetWvW.get_Value());
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/deaths_week.txt", $"{prefixDeaths}{SessionDeathsWvW.get_Value()}{suffixDeaths}");
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/deaths_day.txt", $"{prefixDeaths}{SessionDeathsDaily.get_Value()}{suffixDeaths}");
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
			GameService.Gw2Mumble.get_PlayerCharacter().remove_NameChanged((EventHandler<ValueEventArgs<string>>)OnNameChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_SpecializationChanged((EventHandler<ValueEventArgs<int>>)OnSpecializationChanged);
			UseCatmanderTag.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnUseCatmanderTagSettingChanged);
			GameService.Gw2Mumble.get_PlayerCharacter().remove_IsCommanderChanged((EventHandler<ValueEventArgs<bool>>)OnIsCommanderChanged);
		}
	}
}

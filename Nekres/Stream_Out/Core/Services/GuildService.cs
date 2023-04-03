using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Extended;
using Blish_HUD.Modules.Managers;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Nekres.Stream_Out.Core.Services
{
	internal class GuildService : ExportService
	{
		private const string GUILD_NAME = "guild_name.txt";

		private const string GUILD_TAG = "guild_tag.txt";

		private const string GUILD_EMBLEM = "guild_emblem.png";

		private const string GUILD_MOTD = "guild_motd.txt";

		private Regex GUILD_MOTD_PUBLIC = new Regex("(?<=\\[public\\]).*(?=\\[\\/public\\])", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

		private Gw2ApiManager Gw2ApiManager => StreamOutModule.Instance?.Gw2ApiManager;

		private DirectoriesManager DirectoriesManager => StreamOutModule.Instance?.DirectoriesManager;

		public GuildService(SettingCollection settings)
			: base(settings)
		{
		}

		private async Task UpdateGuild()
		{
			if (!GameService.Gw2Mumble.get_IsAvailable())
			{
				return;
			}
			Gw2ApiManager gw2ApiManager = Gw2ApiManager;
			TokenPermission[] array = new TokenPermission[3];
			RuntimeHelpers.InitializeArray(array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			if (!gw2ApiManager.HasPermissions((IEnumerable<TokenPermission>)(object)array))
			{
				return;
			}
			CharactersCore obj = await TaskUtil.RetryAsync(() => ((IBlobClient<CharactersCore>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()
				.get_Item(GameService.Gw2Mumble.get_PlayerCharacter().get_Name())
				.get_Core()).GetAsync(default(CancellationToken)));
			Guid guildId = ((obj != null) ? obj.get_Guild() : Guid.Empty);
			if (guildId.Equals(Guid.Empty))
			{
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_name.txt", string.Empty);
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_tag.txt", string.Empty);
				await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_motd.txt", string.Empty);
				await TextureUtil.ClearImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_emblem.png");
				return;
			}
			await ((IBlobClient<Guild>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Guild()
				.get_Item(guildId)).GetAsync(default(CancellationToken)).ContinueWith((Func<Task<Guild>, Task>)async delegate(Task<Guild> task)
			{
				if (!task.IsFaulted)
				{
					string name = task.Result.get_Name();
					string tag = task.Result.get_Tag();
					string motd = task.Result.get_Motd() ?? string.Empty;
					await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_name.txt", name);
					await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_tag.txt", "[" + tag + "]");
					await FileUtil.WriteAllTextAsync(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_motd.txt", GUILD_MOTD_PUBLIC.Match(motd).Value);
					GuildEmblem emblem = task.Result.get_Emblem();
					if (emblem == null)
					{
						await TextureUtil.ClearImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_emblem.png");
					}
					else
					{
						Emblem bg = await TaskUtil.RetryAsync(() => ((IBulkExpandableClient<Emblem, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Emblem()
							.get_Backgrounds()).GetAsync(emblem.get_Background().get_Id(), default(CancellationToken)));
						Emblem fg = await TaskUtil.RetryAsync(() => ((IBulkExpandableClient<Emblem, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Emblem()
							.get_Foregrounds()).GetAsync(emblem.get_Foreground().get_Id(), default(CancellationToken)));
						List<RenderUrl> layersCombined = new List<RenderUrl>();
						if (bg != null)
						{
							layersCombined.AddRange(bg.get_Layers());
						}
						if (fg != null)
						{
							layersCombined.AddRange(fg.get_Layers().Skip(1));
						}
						List<Bitmap> layers = new List<Bitmap>();
						foreach (RenderUrl item in layersCombined)
						{
							RenderUrl renderUrl = item;
							using MemoryStream textureStream = new MemoryStream();
							await ((RenderUrl)(ref renderUrl)).DownloadToStreamAsync((Stream)textureStream, default(CancellationToken));
							layers.Add(new Bitmap(textureStream));
						}
						if (!layers.Any())
						{
							await TextureUtil.ClearImage(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_emblem.png");
						}
						else
						{
							List<int> colorsCombined = new List<int>();
							colorsCombined.AddRange(emblem.get_Background().get_Colors());
							colorsCombined.AddRange(emblem.get_Foreground().get_Colors());
							List<Color> colors = new List<Color>();
							foreach (int colorId in colorsCombined)
							{
								Color color = await TaskUtil.RetryAsync(() => ((IBulkExpandableClient<Color, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Colors()).GetAsync(colorId, default(CancellationToken)));
								if (color == null)
								{
									return;
								}
								colors.Add(Color.FromArgb(color.get_Cloth().get_Rgb()[0], color.get_Cloth().get_Rgb()[1], color.get_Cloth().get_Rgb()[2]));
							}
							Bitmap result = new Bitmap(256, 256);
							for (int i = 0; i < layers.Count; i++)
							{
								Bitmap layer = layers[i].FitTo(result);
								layer.Colorize(colors[i]);
								if (bg != null && i < bg.get_Layers().Count)
								{
									layer.Flip(((IEnumerable<ApiEnum<GuildEmblemFlag>>)emblem.get_Flags()).Any((ApiEnum<GuildEmblemFlag> x) => x == ApiEnum<GuildEmblemFlag>.op_Implicit((GuildEmblemFlag)0)), ((IEnumerable<ApiEnum<GuildEmblemFlag>>)emblem.get_Flags()).Any((ApiEnum<GuildEmblemFlag> x) => x == ApiEnum<GuildEmblemFlag>.op_Implicit((GuildEmblemFlag)1)));
								}
								else
								{
									layer.Flip(((IEnumerable<ApiEnum<GuildEmblemFlag>>)emblem.get_Flags()).Any((ApiEnum<GuildEmblemFlag> x) => x == ApiEnum<GuildEmblemFlag>.op_Implicit((GuildEmblemFlag)2)), ((IEnumerable<ApiEnum<GuildEmblemFlag>>)emblem.get_Flags()).Any((ApiEnum<GuildEmblemFlag> x) => x == ApiEnum<GuildEmblemFlag>.op_Implicit((GuildEmblemFlag)3)));
								}
								Bitmap bitmap = result.Merge(layer);
								result.Dispose();
								layer.Dispose();
								result = bitmap;
							}
							result?.Save(DirectoriesManager.GetFullDirectoryPath("stream_out") + "/guild_emblem.png", ImageFormat.Png);
						}
					}
				}
			});
		}

		protected override async Task Update()
		{
			await UpdateGuild();
		}

		public override async Task Clear()
		{
			string dir = DirectoriesManager.GetFullDirectoryPath("stream_out");
			await FileUtil.DeleteAsync(Path.Combine(dir, "guild_name.txt"));
			await FileUtil.DeleteAsync(Path.Combine(dir, "guild_tag.txt"));
			await FileUtil.DeleteAsync(Path.Combine(dir, "guild_emblem.png"));
			await FileUtil.DeleteAsync(Path.Combine(dir, "guild_motd.txt"));
		}

		public override void Dispose()
		{
		}
	}
}

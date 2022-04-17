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
using Blish_HUD.Modules.Managers;
using Gw2Sharp.WebApi;
using Gw2Sharp.WebApi.V2.Clients;
using Gw2Sharp.WebApi.V2.Models;

namespace Nekres.Stream_Out.Core.Services
{
	internal class GuildService : IExportService, IDisposable
	{
		private const string GUILD_NAME = "guild_name.txt";

		private const string GUILD_TAG = "guild_tag.txt";

		private const string GUILD_EMBLEM = "guild_emblem.png";

		private const string GUILD_MOTD = "guild_motd.txt";

		private Regex GUILD_MOTD_PUBLIC = new Regex("(?<=\\[public\\]).*(?=\\[\\/public\\])", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

		private Logger Logger => StreamOutModule.Logger;

		private Gw2ApiManager Gw2ApiManager => StreamOutModule.ModuleInstance?.Gw2ApiManager;

		private DirectoriesManager DirectoriesManager => StreamOutModule.ModuleInstance?.DirectoriesManager;

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
			Guid guildId = await ((IBlobClient<CharactersCore>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Characters()
				.get_Item(GameService.Gw2Mumble.get_PlayerCharacter().get_Name())
				.get_Core()).GetAsync(default(CancellationToken)).ContinueWith((Task<CharactersCore> task) => (!task.IsFaulted) ? task.Result.get_Guild() : Guid.Empty);
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
						Emblem bg = await ((IBulkExpandableClient<Emblem, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Emblem()
							.get_Backgrounds()).GetAsync(emblem.get_Background().get_Id(), default(CancellationToken));
						Emblem fg = await ((IBulkExpandableClient<Emblem, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Emblem()
							.get_Foregrounds()).GetAsync(emblem.get_Foreground().get_Id(), default(CancellationToken));
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
							foreach (int color2 in colorsCombined)
							{
								List<Color> list = colors;
								list.Add(await ((IBulkExpandableClient<Color, int>)(object)Gw2ApiManager.get_Gw2ApiClient().get_V2().get_Colors()).GetAsync(color2, default(CancellationToken)));
							}
							Bitmap result = new Bitmap(256, 256);
							for (int i = 0; i < layers.Count; i++)
							{
								Bitmap layer = layers[i].FitTo(result);
								if (colors.Any())
								{
									Color color = Color.FromArgb(colors[i].get_Cloth().get_Rgb()[0], colors[i].get_Cloth().get_Rgb()[1], colors[i].get_Cloth().get_Rgb()[2]);
									layer.Colorize(color);
								}
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

		public async Task Update()
		{
			await UpdateGuild();
		}

		public async Task Initialize()
		{
		}

		public async Task ResetDaily()
		{
		}

		public void Dispose()
		{
		}
	}
}

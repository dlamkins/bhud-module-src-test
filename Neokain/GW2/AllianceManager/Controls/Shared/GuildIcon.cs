using System;
using System.IO;
using System.Threading.Tasks;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework.Graphics;
using Neokain.GW2.WebClient;
using Neokain.GW2.WebClient.Models.Guilds;

namespace Neokain.GW2.AllianceManager.Controls.Shared
{
	internal class GuildIcon : CustomIcon
	{
		private static readonly Logger Logger = Logger.GetLogger<GuildIcon>();

		private readonly Guid _guildId;

		private readonly Gw2WebClient _webClient;

		private bool _loadAttempted;

		public GuildIcon(GuildDetailDto guild, Gw2WebClient webClient)
		{
			if (guild == null)
			{
				throw new ArgumentNullException("guild");
			}
			_guildId = guild.Id;
			_webClient = webClient ?? throw new ArgumentNullException("webClient");
			((Image)this).set_Texture(AsyncTexture2D.FromAssetId(155052));
			LoadEmblemAsync();
		}

		private async Task LoadEmblemAsync()
		{
			if (_loadAttempted)
			{
				return;
			}
			_loadAttempted = true;
			try
			{
				byte[] pngBytes = await _webClient.GetGuildEmblemAsync(_guildId, 64);
				if (pngBytes == null || pngBytes.Length == 0)
				{
					Logger.Debug($"No emblem available for guild {_guildId}");
					return;
				}
				GameService.Graphics.QueueMainThreadRender((Action<GraphicsDevice>)delegate(GraphicsDevice device)
				{
					try
					{
						using MemoryStream memoryStream = new MemoryStream(pngBytes);
						Texture2D val = Texture2D.FromStream(device, (Stream)memoryStream);
						((Image)this).set_Texture(AsyncTexture2D.op_Implicit(val));
					}
					catch (Exception ex2)
					{
						Logger.Warn(ex2, $"Failed to create texture for guild {_guildId}");
					}
				});
			}
			catch (Exception ex)
			{
				Logger.Warn(ex, $"Failed to load emblem for guild {_guildId}");
			}
		}
	}
}

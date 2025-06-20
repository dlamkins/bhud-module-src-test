using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;
using Soeed.GuildGeoGuesser.Feature.Shared.Services;

namespace Soeed.GuildGeoGuesser.Settings.Services
{
	public class TextureService : IDisposable
	{
		private Random _random = new Random();

		public Texture2D SettingWindowBackground;

		public AsyncTexture2D Emblem;

		public AsyncTexture2D BlishWaterColor;

		public AsyncTexture2D PlayIcon;

		public AsyncTexture2D LeaderboardIcon;

		protected DownloadTextureService _downloadTextures { get; set; }

		public TextureService(ContentsManager contentsManager)
		{
			_downloadTextures = new DownloadTextureService();
			SettingWindowBackground = contentsManager.GetTexture("background.png");
			Emblem = AsyncTexture2D.op_Implicit(contentsManager.GetTexture("460029.png"));
			BlishWaterColor = AsyncTexture2D.op_Implicit(contentsManager.GetTexture("blish-watercolor.png"));
			PlayIcon = AsyncTexture2D.op_Implicit(contentsManager.GetTexture("play_icon.png"));
			LeaderboardIcon = AsyncTexture2D.op_Implicit(contentsManager.GetTexture("leaderboard_icon.png"));
		}

		public void ResetDownloadCache()
		{
			_downloadTextures.ResetDownloadCache();
		}

		public void ReleaseTexture(string key)
		{
			_downloadTextures.ReleaseTexture(key);
		}

		public void ForceRefreshTexture(string fileName)
		{
			_downloadTextures.ForceRefreshTexture(fileName);
		}

		public AsyncTexture2D GetDynamicTexture(string path)
		{
			return _downloadTextures.GetDynamicTexture(path);
		}

		public AsyncTexture2D GetURLTexture(string url, string fileName)
		{
			return _downloadTextures.GetDynamicTextureFromUrl(url, fileName);
		}

		public AsyncTexture2D DatAsset(int id)
		{
			if (id == 0)
			{
				List<int> golemIcons = new List<int> { 240696, 240697, 240686 };
				id = golemIcons[_random.Next(golemIcons.Count())];
			}
			return GameService.Content.get_DatAssetCache().GetTextureFromAssetId(id);
		}

		public void Dispose()
		{
			Texture2D settingWindowBackground = SettingWindowBackground;
			if (settingWindowBackground != null)
			{
				((GraphicsResource)settingWindowBackground).Dispose();
			}
			AsyncTexture2D emblem = Emblem;
			if (emblem != null)
			{
				emblem.Dispose();
			}
			AsyncTexture2D blishWaterColor = BlishWaterColor;
			if (blishWaterColor != null)
			{
				blishWaterColor.Dispose();
			}
			AsyncTexture2D playIcon = PlayIcon;
			if (playIcon != null)
			{
				playIcon.Dispose();
			}
			AsyncTexture2D leaderboardIcon = LeaderboardIcon;
			if (leaderboardIcon != null)
			{
				leaderboardIcon.Dispose();
			}
			_downloadTextures.Dispose();
		}
	}
}

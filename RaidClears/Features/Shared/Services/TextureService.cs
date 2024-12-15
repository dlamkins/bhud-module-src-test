using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace RaidClears.Features.Shared.Services
{
	public class TextureService : IDisposable
	{
		protected DownloadTextureService _downloadTextures { get; set; }

		public Texture2D JWLogo { get; }

		public Texture2D PoFLogo { get; }

		public Texture2D HoTLogo { get; }

		public Texture2D BaseLogo { get; }

		public Texture2D SettingWindowBackground { get; }

		public Texture2D SettingWindowEmblem { get; }

		public AsyncTexture2D SettingTabRaid { get; }

		public AsyncTexture2D SettingTabDungeon { get; }

		public Texture2D SettingTabGeneral { get; }

		public AsyncTexture2D SettingTabStrikes { get; }

		public AsyncTexture2D SettingTabFractals { get; }

		public Texture2D CornerIconTexture { get; }

		public Texture2D CornerIconHoverTexture { get; }

		public List<AsyncTexture2D> GridBoxBackgroundTexture { get; private set; } = new List<AsyncTexture2D>();


		public TextureService(ContentsManager contentsManager)
		{
			_downloadTextures = new DownloadTextureService();
			CornerIconTexture = contentsManager.GetTexture("raids\\textures\\raidIconDark.png");
			CornerIconHoverTexture = contentsManager.GetTexture("raids\\textures\\raidIconBright.png");
			SettingWindowBackground = AsyncTexture2D.op_Implicit(GetDynamicTexture("texture_background.png"));
			SettingWindowEmblem = contentsManager.GetTexture("module_profile_hero_icon.png");
			SettingTabRaid = DatAsset(1302679);
			SettingTabDungeon = DatAsset(602776);
			SettingTabGeneral = contentsManager.GetTexture("controls/tab_icons/cog.png");
			SettingTabStrikes = DatAsset(2271016);
			SettingTabFractals = DatAsset(1228226);
			JWLogo = AsyncTexture2D.op_Implicit(GetDynamicTexture("texture_raids_jw.png"));
			PoFLogo = AsyncTexture2D.op_Implicit(GetDynamicTexture("texture_raids_pof.png"));
			HoTLogo = AsyncTexture2D.op_Implicit(GetDynamicTexture("texture_raids_hot.png"));
			BaseLogo = AsyncTexture2D.op_Implicit(GetDynamicTexture("texture_base_logo.png"));
		}

		public void AddGridBoxMask(string path)
		{
			GridBoxBackgroundTexture.Add(GetDynamicTexture(path));
		}

		public AsyncTexture2D GetRandomGridBoxMask()
		{
			if (GridBoxBackgroundTexture.Count > 0)
			{
				return GridBoxBackgroundTexture[Service.Random.Next(GridBoxBackgroundTexture.Count)];
			}
			return AsyncTexture2D.op_Implicit(Textures.get_Pixel());
		}

		public AsyncTexture2D GetDynamicTexture(string path)
		{
			return AsyncTexture2D.op_Implicit(_downloadTextures.GetDynamicTexture(path));
		}

		public AsyncTexture2D DatAsset(int id)
		{
			if (id == 0)
			{
				List<int> golemIcons = new List<int> { 240696, 240697, 240686 };
				id = golemIcons[new Random().Next(golemIcons.Count())];
			}
			return GameService.Content.get_DatAssetCache().GetTextureFromAssetId(id);
		}

		public void Dispose()
		{
			GridBoxBackgroundTexture.Clear();
			((GraphicsResource)CornerIconTexture).Dispose();
			((GraphicsResource)CornerIconHoverTexture).Dispose();
			((GraphicsResource)SettingWindowBackground).Dispose();
			((GraphicsResource)SettingWindowEmblem).Dispose();
			((GraphicsResource)SettingTabGeneral).Dispose();
			((GraphicsResource)JWLogo).Dispose();
			((GraphicsResource)PoFLogo).Dispose();
			((GraphicsResource)HoTLogo).Dispose();
			((GraphicsResource)BaseLogo).Dispose();
			_downloadTextures.Dispose();
		}
	}
}

using System;
using Blish_HUD.Content;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace RaidClears.Features.Shared.Services
{
	public class TextureService : IDisposable
	{
		protected DownloadTextureService _downloadTextures { get; set; }

		public Texture2D PoFLogo { get; }

		public Texture2D HoTLogo { get; }

		public Texture2D BaseLogo { get; }

		public Texture2D SettingWindowBackground { get; }

		public Texture2D SettingWindowEmblem { get; }

		public Texture2D SettingTabRaid { get; }

		public Texture2D SettingTabDungeon { get; }

		public Texture2D SettingTabGeneral { get; }

		public Texture2D SettingTabStrikes { get; }

		public Texture2D SettingTabFractals { get; }

		public Texture2D CornerIconTexture { get; }

		public Texture2D CornerIconHoverTexture { get; }

		public TextureService(ContentsManager contentsManager)
		{
			_downloadTextures = new DownloadTextureService();
			CornerIconTexture = contentsManager.GetTexture("raids\\textures\\raidIconDark.png");
			CornerIconHoverTexture = contentsManager.GetTexture("raids\\textures\\raidIconBright.png");
			SettingWindowBackground = AsyncTexture2D.op_Implicit(GetDynamicTexture("texture_background.png"));
			SettingWindowEmblem = contentsManager.GetTexture("module_profile_hero_icon.png");
			SettingTabRaid = contentsManager.GetTexture("controls/tab_icons/raid.png");
			SettingTabDungeon = contentsManager.GetTexture("controls/tab_icons/dungeon.png");
			SettingTabGeneral = contentsManager.GetTexture("controls/tab_icons/cog.png");
			SettingTabStrikes = contentsManager.GetTexture("controls/tab_icons/strikes.png");
			SettingTabFractals = contentsManager.GetTexture("controls/tab_icons/fotm.png");
			PoFLogo = AsyncTexture2D.op_Implicit(GetDynamicTexture("texture_raids_pof.png"));
			HoTLogo = AsyncTexture2D.op_Implicit(GetDynamicTexture("texture_raids_hot.png"));
			BaseLogo = AsyncTexture2D.op_Implicit(GetDynamicTexture("texture_base_logo.png"));
		}

		public AsyncTexture2D GetDynamicTexture(string path)
		{
			return AsyncTexture2D.op_Implicit(_downloadTextures.GetDynamicTexture(path));
		}

		public void Dispose()
		{
			((GraphicsResource)CornerIconTexture).Dispose();
			((GraphicsResource)CornerIconHoverTexture).Dispose();
			((GraphicsResource)SettingWindowBackground).Dispose();
			((GraphicsResource)SettingWindowEmblem).Dispose();
			((GraphicsResource)SettingTabRaid).Dispose();
			((GraphicsResource)SettingTabDungeon).Dispose();
			((GraphicsResource)SettingTabGeneral).Dispose();
			((GraphicsResource)SettingTabStrikes).Dispose();
			((GraphicsResource)SettingTabFractals).Dispose();
			((GraphicsResource)PoFLogo).Dispose();
			((GraphicsResource)HoTLogo).Dispose();
			((GraphicsResource)BaseLogo).Dispose();
			_downloadTextures.Dispose();
		}
	}
}

using System;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace RaidClears.Features.Shared.Services
{
	public class TextureService : IDisposable
	{
		public Texture2D EoDLogo { get; }

		public Texture2D IBSLogo { get; }

		public Texture2D PoFLogo { get; }

		public Texture2D HoTLogo { get; }

		public Texture2D BaseLogo { get; }

		public Texture2D SettingWindowBackground { get; }

		public Texture2D SettingWindowEmblem { get; }

		public Texture2D SettingTabRaid { get; }

		public Texture2D SettingTabDungeon { get; }

		public Texture2D SettingTabGeneral { get; }

		public Texture2D SettingTabStrikes { get; }

		public Texture2D CornerIconTexture { get; }

		public Texture2D CornerIconHoverTexture { get; }

		public TextureService(ContentsManager contentsManager)
		{
			CornerIconTexture = contentsManager.GetTexture("raids\\textures\\raidIconDark.png");
			CornerIconHoverTexture = contentsManager.GetTexture("raids\\textures\\raidIconBright.png");
			SettingWindowBackground = contentsManager.GetTexture("controls/window/background.png");
			SettingWindowEmblem = contentsManager.GetTexture("module_profile_hero_icon.png");
			SettingTabRaid = contentsManager.GetTexture("controls/tab_icons/raid.png");
			SettingTabDungeon = contentsManager.GetTexture("controls/tab_icons/dungeon.png");
			SettingTabGeneral = contentsManager.GetTexture("controls/tab_icons/cog.png");
			SettingTabStrikes = contentsManager.GetTexture("controls/tab_icons/strikes.png");
			EoDLogo = contentsManager.GetTexture("eod_strikes_texture.png");
			IBSLogo = contentsManager.GetTexture("ibs_strikes_texture.png");
			PoFLogo = contentsManager.GetTexture("pof_raids_texture.png");
			HoTLogo = contentsManager.GetTexture("hot_raids_texture.png");
			BaseLogo = contentsManager.GetTexture("base_game_texture.png");
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
			((GraphicsResource)EoDLogo).Dispose();
			((GraphicsResource)IBSLogo).Dispose();
			((GraphicsResource)PoFLogo).Dispose();
			((GraphicsResource)HoTLogo).Dispose();
			((GraphicsResource)BaseLogo).Dispose();
		}
	}
}

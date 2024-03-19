using System;
using System.Collections.Generic;
using Blish_HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Nekres.ChatMacros.Core.Services
{
	internal class ResourceService : IDisposable
	{
		public Color BrightGold = new Color(223, 194, 149, 255);

		public List<string> Placeholders = new List<string>
		{
			"{wp} - Closest waypoint.", "{poi} - Closest Point of Interest", "{blish} - Your Blish HUD version", "{random} - A random number.", "{random :max} - A random number from 0 to max.", "{random :min :max} - A random number between min and max.", "{json :property.path :url} - Makes a web request to the specified URL and pulls a value found at the specified path from a JSON response.", "{txt :filepath} - A random line from the given file.", "{txt :filepath :line} - A specific line from the given file. Filepaths can be absolute, relative to Blish HUD.exe or relative to the chat_shorts module directory.", "{today} - Today's date.",
			"{time} - Current local time.", "{map} - Current map name."
		};

		private IReadOnlyList<SoundEffect> _menuClicks;

		private SoundEffect _menuItemClickSfx;

		public Texture2D DragReorderIcon { get; private set; }

		public Texture2D TwitchLogo { get; private set; }

		public Texture2D YoutubeLogo { get; private set; }

		public Texture2D EditIcon { get; private set; }

		public Texture2D LinkIcon { get; private set; }

		public Texture2D LinkBrokenIcon { get; private set; }

		public Texture2D OpenExternalIcon { get; private set; }

		public Texture2D SwitchModeOnIcon { get; private set; }

		public Texture2D SwitchModeOffIcon { get; private set; }

		public BitmapFont RubikRegular26 { get; private set; }

		public BitmapFont LatoRegular24 { get; private set; }

		public BitmapFont SourceCodePro24 { get; private set; }

		public ResourceService()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			LoadTextures();
			LoadFonts();
			LoadSounds();
		}

		public void LoadTextures()
		{
			DragReorderIcon = ChatMacros.Instance.ContentsManager.GetTexture("icons/drag-reorder.png");
			EditIcon = ChatMacros.Instance.ContentsManager.GetTexture("icons/edit_icon.png");
			LinkIcon = ChatMacros.Instance.ContentsManager.GetTexture("icons/link.png");
			LinkBrokenIcon = ChatMacros.Instance.ContentsManager.GetTexture("icons/link-broken.png");
			OpenExternalIcon = ChatMacros.Instance.ContentsManager.GetTexture("icons/open-external.png");
			SwitchModeOnIcon = ChatMacros.Instance.ContentsManager.GetTexture("icons/switch-mode-on.png");
			SwitchModeOffIcon = ChatMacros.Instance.ContentsManager.GetTexture("icons/switch-mode-off.png");
			TwitchLogo = ChatMacros.Instance.ContentsManager.GetTexture("socials/twitch_logo.png");
			YoutubeLogo = ChatMacros.Instance.ContentsManager.GetTexture("socials/youtube_logo.png");
		}

		public void PlayMenuItemClick()
		{
			_menuItemClickSfx.Play(GameService.GameIntegration.get_Audio().get_Volume(), 0f, 0f);
		}

		public void PlayMenuClick()
		{
			_menuClicks[RandomUtil.GetRandom(0, 3)].Play(GameService.GameIntegration.get_Audio().get_Volume(), 0f, 0f);
		}

		private void LoadFonts()
		{
			RubikRegular26 = ChatMacros.Instance.ContentsManager.GetBitmapFont("fonts/Rubik-Regular.ttf", 26);
			LatoRegular24 = ChatMacros.Instance.ContentsManager.GetBitmapFont("fonts/Lato-Regular.ttf", 24);
			SourceCodePro24 = ChatMacros.Instance.ContentsManager.GetBitmapFont("fonts/SourceCodePro-SemiBold.ttf", 24);
		}

		private void LoadSounds()
		{
			_menuItemClickSfx = ChatMacros.Instance.ContentsManager.GetSound("audio\\menu-item-click.wav");
			_menuClicks = new List<SoundEffect>
			{
				ChatMacros.Instance.ContentsManager.GetSound("audio\\menu-click-1.wav"),
				ChatMacros.Instance.ContentsManager.GetSound("audio\\menu-click-2.wav"),
				ChatMacros.Instance.ContentsManager.GetSound("audio\\menu-click-3.wav"),
				ChatMacros.Instance.ContentsManager.GetSound("audio\\menu-click-4.wav")
			};
		}

		public void Dispose()
		{
			RubikRegular26?.Dispose();
			LatoRegular24?.Dispose();
			SourceCodePro24?.Dispose();
			Texture2D dragReorderIcon = DragReorderIcon;
			if (dragReorderIcon != null)
			{
				((GraphicsResource)dragReorderIcon).Dispose();
			}
			Texture2D editIcon = EditIcon;
			if (editIcon != null)
			{
				((GraphicsResource)editIcon).Dispose();
			}
			Texture2D linkIcon = LinkIcon;
			if (linkIcon != null)
			{
				((GraphicsResource)linkIcon).Dispose();
			}
			Texture2D linkBrokenIcon = LinkBrokenIcon;
			if (linkBrokenIcon != null)
			{
				((GraphicsResource)linkBrokenIcon).Dispose();
			}
			Texture2D openExternalIcon = OpenExternalIcon;
			if (openExternalIcon != null)
			{
				((GraphicsResource)openExternalIcon).Dispose();
			}
			Texture2D switchModeOnIcon = SwitchModeOnIcon;
			if (switchModeOnIcon != null)
			{
				((GraphicsResource)switchModeOnIcon).Dispose();
			}
			Texture2D switchModeOffIcon = SwitchModeOffIcon;
			if (switchModeOffIcon != null)
			{
				((GraphicsResource)switchModeOffIcon).Dispose();
			}
			Texture2D twitchLogo = TwitchLogo;
			if (twitchLogo != null)
			{
				((GraphicsResource)twitchLogo).Dispose();
			}
			Texture2D youtubeLogo = YoutubeLogo;
			if (youtubeLogo != null)
			{
				((GraphicsResource)youtubeLogo).Dispose();
			}
		}
	}
}

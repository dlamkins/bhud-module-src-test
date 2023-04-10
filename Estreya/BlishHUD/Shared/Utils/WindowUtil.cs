using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Estreya.BlishHUD.Shared.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Utils
{
	public static class WindowUtil
	{
		public static StandardWindow CreateStandardWindow(string title, Type callingType, Guid guid, IconState iconState, AsyncTexture2D emblem = null)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Expected O, but got Unknown
			string backgroundTexturePath = "textures\\setting_window_background.png";
			Texture2D windowBackground = AsyncTexture2D.op_Implicit(iconState?.GetIcon(backgroundTexturePath));
			if (windowBackground == null || windowBackground == Textures.get_Error())
			{
				throw new ArgumentNullException("windowBackground", "Module does not include texture \"" + backgroundTexturePath + "\".");
			}
			Rectangle settingsWindowSize = default(Rectangle);
			((Rectangle)(ref settingsWindowSize))._002Ector(35, 26, 1100, 714);
			int contentRegionPaddingY = settingsWindowSize.Y - 15;
			int contentRegionPaddingX = settingsWindowSize.X;
			Rectangle contentRegion = default(Rectangle);
			((Rectangle)(ref contentRegion))._002Ector(contentRegionPaddingX, contentRegionPaddingY, settingsWindowSize.Width - 6, settingsWindowSize.Height - contentRegionPaddingY);
			StandardWindow val = new StandardWindow(windowBackground, settingsWindowSize, contentRegion);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title(title);
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id($"{callingType.Name}_{guid}");
			StandardWindow window = val;
			if (emblem != null)
			{
				if (emblem.get_HasSwapped())
				{
					((WindowBase2)window).set_Emblem(AsyncTexture2D.op_Implicit(emblem));
				}
				else
				{
					emblem.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)delegate(object s, ValueChangedEventArgs<Texture2D> e)
					{
						((WindowBase2)window).set_Emblem(e.get_NewValue());
					});
				}
			}
			return window;
		}

		public static TabbedWindow2 CreateTabbedWindow(string title, Type callingType, Guid guid, IconState iconState, AsyncTexture2D emblem = null)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Expected O, but got Unknown
			string backgroundTexturePath = "textures\\setting_window_background.png";
			Texture2D windowBackground = AsyncTexture2D.op_Implicit(iconState?.GetIcon(backgroundTexturePath));
			if (windowBackground == null || windowBackground == Textures.get_Error())
			{
				throw new ArgumentNullException("windowBackground", "Module does not include texture \"" + backgroundTexturePath + "\".");
			}
			Rectangle settingsWindowSize = default(Rectangle);
			((Rectangle)(ref settingsWindowSize))._002Ector(35, 26, 1100, 714);
			int contentRegionPaddingY = settingsWindowSize.Y - 15;
			int contentRegionPaddingX = settingsWindowSize.X + 46;
			Rectangle contentRegion = default(Rectangle);
			((Rectangle)(ref contentRegion))._002Ector(contentRegionPaddingX, contentRegionPaddingY, settingsWindowSize.Width - 52, settingsWindowSize.Height - contentRegionPaddingY);
			TabbedWindow2 val = new TabbedWindow2(windowBackground, settingsWindowSize, contentRegion);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			((WindowBase2)val).set_Title(title);
			((WindowBase2)val).set_SavesPosition(true);
			((WindowBase2)val).set_Id($"{callingType.Name}_{guid}");
			TabbedWindow2 window = val;
			if (emblem != null)
			{
				if (emblem.get_HasSwapped())
				{
					((WindowBase2)window).set_Emblem(AsyncTexture2D.op_Implicit(emblem));
				}
				else
				{
					emblem.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)delegate(object s, ValueChangedEventArgs<Texture2D> e)
					{
						((WindowBase2)window).set_Emblem(e.get_NewValue());
					});
				}
			}
			return window;
		}
	}
}

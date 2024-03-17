using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Estreya.BlishHUD.Shared.Controls;
using Estreya.BlishHUD.Shared.Services;
using Estreya.BlishHUD.Shared.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Estreya.BlishHUD.Shared.Utils
{
	public static class WindowUtil
	{
		private static AsyncTexture2D GetWindowBackgroundTexture(IconService iconService)
		{
			return iconService?.GetIcon("502049.png");
		}

		private static Rectangle GetDefaultWindowSize()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			return new Rectangle(35, 26, 930, 710);
		}

		public static StandardWindow CreateStandardWindow(BaseModuleSettings moduleSettings, string title, Type callingType, Guid guid, IconService iconService, AsyncTexture2D emblem = null)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D windowBackground = GetWindowBackgroundTexture(iconService);
			Rectangle settingsWindowSize = GetDefaultWindowSize();
			int contentRegionPaddingY = settingsWindowSize.Y - 15;
			int contentRegionPaddingX = settingsWindowSize.X;
			Rectangle contentRegion = default(Rectangle);
			((Rectangle)(ref contentRegion))._002Ector(contentRegionPaddingX, contentRegionPaddingY, settingsWindowSize.Width - 6, settingsWindowSize.Height - contentRegionPaddingY);
			StandardWindow standardWindow = new StandardWindow(moduleSettings, windowBackground, settingsWindowSize, contentRegion);
			((Control)standardWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			standardWindow.Title = title;
			standardWindow.SavesPosition = true;
			standardWindow.Id = $"{callingType.Name}_{guid}";
			QueueEmblemChange(standardWindow, emblem);
			return standardWindow;
		}

		public static TabbedWindow CreateTabbedWindow(BaseModuleSettings moduleSettings, string title, Type callingType, Guid guid, IconService iconService, AsyncTexture2D emblem = null)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			AsyncTexture2D windowBackground = GetWindowBackgroundTexture(iconService);
			Rectangle settingsWindowSize = GetDefaultWindowSize();
			int contentRegionPaddingY = settingsWindowSize.Y - 15;
			int contentRegionPaddingX = settingsWindowSize.X + 46;
			Rectangle contentRegion = default(Rectangle);
			((Rectangle)(ref contentRegion))._002Ector(contentRegionPaddingX, contentRegionPaddingY, settingsWindowSize.Width - 46, settingsWindowSize.Height);
			TabbedWindow tabbedWindow = new TabbedWindow(moduleSettings, windowBackground, settingsWindowSize, contentRegion);
			((Control)tabbedWindow).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			tabbedWindow.Title = title;
			tabbedWindow.SavesPosition = true;
			tabbedWindow.Id = $"{callingType.Name}_{guid}";
			QueueEmblemChange(tabbedWindow, emblem);
			return tabbedWindow;
		}

		private static void QueueEmblemChange(Window window, AsyncTexture2D emblem)
		{
			if (emblem == null)
			{
				return;
			}
			if (emblem.get_HasSwapped())
			{
				window.Emblem = AsyncTexture2D.op_Implicit(emblem);
				return;
			}
			emblem.add_TextureSwapped((EventHandler<ValueChangedEventArgs<Texture2D>>)delegate(object s, ValueChangedEventArgs<Texture2D> e)
			{
				window.Emblem = e.get_NewValue();
			});
		}
	}
}

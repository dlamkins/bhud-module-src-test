using System;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using GatheringTools.ToolSearch.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace GatheringTools.ToolSearch.Services
{
	public class CornerIconService
	{
		private readonly ToolSearchStandardWindow _toolSearchStandardWindow;

		private readonly Texture2D _cornerIconTexture;

		private CornerIcon _toolSearchCornerIcon;

		public CornerIconService(SettingEntry<bool> showToolSearchCornerIconSetting, ToolSearchStandardWindow toolSearchStandardWindow, Texture2D cornerIconTexture)
		{
			_toolSearchStandardWindow = toolSearchStandardWindow;
			_cornerIconTexture = cornerIconTexture;
			if (showToolSearchCornerIconSetting.get_Value())
			{
				CreateCornerIcon();
			}
			showToolSearchCornerIconSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)delegate(object s, ValueChangedEventArgs<bool> e)
			{
				if (e.get_NewValue())
				{
					CreateCornerIcon();
				}
				else
				{
					RemoveCornerIcon();
				}
			});
		}

		public void RemoveCornerIcon()
		{
			CornerIcon toolSearchCornerIcon = _toolSearchCornerIcon;
			if (toolSearchCornerIcon != null)
			{
				((Control)toolSearchCornerIcon).Dispose();
			}
			_toolSearchCornerIcon = null;
		}

		private void CreateCornerIcon()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(_cornerIconTexture));
			((Control)val).set_BasicTooltipText("Click to show/hide which character has gathering tools equipped.\nIcon can be hidden by module settings.");
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			_toolSearchCornerIcon = val;
			((Control)_toolSearchCornerIcon).add_Click((EventHandler<MouseEventArgs>)async delegate
			{
				await _toolSearchStandardWindow.ToggleVisibility();
			});
		}
	}
}

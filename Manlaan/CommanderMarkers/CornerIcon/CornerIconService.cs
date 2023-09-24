using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Graphics;

namespace Manlaan.CommanderMarkers.CornerIcon
{
	public class CornerIconService : IDisposable
	{
		private readonly IEnumerable<ContextMenuStripItem> _contextMenuItems;

		private readonly Texture2D _cornerIconTexture;

		private readonly Texture2D _cornerIconHoverTexture;

		private readonly SettingEntry<bool> _cornerIconIsVisibleSetting;

		private readonly string _tooltip;

		private CornerIcon? _cornerIcon;

		public event EventHandler<bool>? IconLeftClicked;

		public CornerIconService(SettingEntry<bool> cornerIconIsVisibleSetting, string tooltip, Texture2D defaultTexture, Texture2D hoverTexture, IEnumerable<ContextMenuStripItem> contextMenuItems)
		{
			_tooltip = tooltip;
			_cornerIconIsVisibleSetting = cornerIconIsVisibleSetting;
			_cornerIconTexture = defaultTexture;
			_cornerIconHoverTexture = hoverTexture;
			_contextMenuItems = contextMenuItems;
			_cornerIconIsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnCornerIconIsVisibleSettingChanged);
			if (cornerIconIsVisibleSetting.get_Value())
			{
				CreateCornerIcon();
			}
		}

		public void OpenContextMenu()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			CornerIcon? cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				ContextMenuStrip menu = ((Control)cornerIcon).get_Menu();
				if (menu != null)
				{
					menu.Show(GameService.Input.get_Mouse().get_Position());
				}
			}
		}

		public void Dispose()
		{
			_cornerIconIsVisibleSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnCornerIconIsVisibleSettingChanged);
			RemoveCornerIcon();
		}

		private void CreateCornerIcon()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			RemoveCornerIcon();
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(_cornerIconTexture));
			val.set_HoverIcon(AsyncTexture2D.op_Implicit(_cornerIconHoverTexture));
			((Control)val).set_BasicTooltipText(_tooltip);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			val.set_Priority(1122334455);
			_cornerIcon = val;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)OnCornerIconClicked);
			((Control)_cornerIcon).set_Menu((ContextMenuStrip)(object)new CornerIconContextMenu(() => _contextMenuItems));
		}

		private void RemoveCornerIcon()
		{
			if (_cornerIcon == null)
			{
				return;
			}
			((Control)_cornerIcon).remove_Click((EventHandler<MouseEventArgs>)OnCornerIconClicked);
			CornerIcon? cornerIcon = _cornerIcon;
			if (cornerIcon != null)
			{
				ContextMenuStrip menu = ((Control)cornerIcon).get_Menu();
				if (menu != null)
				{
					((Control)menu).Dispose();
				}
			}
			CornerIcon? cornerIcon2 = _cornerIcon;
			if (cornerIcon2 != null)
			{
				((Control)cornerIcon2).Dispose();
			}
		}

		private void OnCornerIconIsVisibleSettingChanged(object sender, ValueChangedEventArgs<bool> e)
		{
			if (e.get_NewValue())
			{
				CreateCornerIcon();
			}
			else
			{
				RemoveCornerIcon();
			}
		}

		private void OnCornerIconClicked(object sender, MouseEventArgs e)
		{
			this.IconLeftClicked?.Invoke(this, e: true);
		}
	}
}

using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Manlaan.CommanderMarkers.Library.Enums;

namespace Manlaan.CommanderMarkers.CornerIcon
{
	public class CornerIconService : IDisposable
	{
		private readonly IEnumerable<ContextMenuStripItem> _contextMenuItems;

		private readonly SettingEntry<bool> _cornerIconIsVisibleSetting;

		private readonly string _tooltip;

		private CornerIcon? _cornerIcon;

		public event EventHandler<bool>? IconLeftClicked;

		public CornerIconService(SettingEntry<bool> cornerIconIsVisibleSetting, string tooltip, IEnumerable<ContextMenuStripItem> contextMenuItems)
		{
			_tooltip = tooltip;
			_cornerIconIsVisibleSetting = cornerIconIsVisibleSetting;
			_contextMenuItems = contextMenuItems;
			_cornerIconIsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnCornerIconIsVisibleSettingChanged);
			Service.Settings.CornerIconPriority.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)CornerIconPriority_SettingChanged);
			Service.Settings.CornerIconTexture.add_SettingChanged((EventHandler<ValueChangedEventArgs<SquadMarker>>)CornerIconTexture_SettingChanged);
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
			Service.Settings.CornerIconPriority.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)CornerIconPriority_SettingChanged);
			Service.Settings.CornerIconTexture.remove_SettingChanged((EventHandler<ValueChangedEventArgs<SquadMarker>>)CornerIconTexture_SettingChanged);
			RemoveCornerIcon();
		}

		private void CreateCornerIcon()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Expected O, but got Unknown
			RemoveCornerIcon();
			CornerIcon val = new CornerIcon();
			val.set_Icon(AsyncTexture2D.op_Implicit(Service.Settings.CornerIconTexture.get_Value().GetFadedIcon()));
			val.set_HoverIcon(AsyncTexture2D.op_Implicit(Service.Settings.CornerIconTexture.get_Value().GetIcon()));
			((Control)val).set_BasicTooltipText(_tooltip);
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			val.set_Priority((int)(2.1474836E+09f * ((1000f - (float)Service.Settings.CornerIconPriority.get_Value()) / 1000f)) - 1);
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

		private void CornerIconTexture_SettingChanged(object sender, ValueChangedEventArgs<SquadMarker> e)
		{
			if (Service.Settings.CornerIconEnabled.get_Value() && _cornerIcon != null)
			{
				_cornerIcon!.set_Icon(AsyncTexture2D.op_Implicit(e.get_NewValue().GetFadedIcon()));
				_cornerIcon!.set_HoverIcon(AsyncTexture2D.op_Implicit(e.get_NewValue().GetIcon()));
			}
		}

		private void CornerIconPriority_SettingChanged(object sender, ValueChangedEventArgs<int> e)
		{
			if (Service.Settings.CornerIconEnabled.get_Value() && _cornerIcon != null)
			{
				_cornerIcon!.set_Priority((int)(2.1474836E+09f * ((1000f - (float)e.get_NewValue()) / 1000f)) - 1);
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

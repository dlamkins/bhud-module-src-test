using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework.Graphics;

namespace RaidClears.Features.Shared.Services
{
	public class CornerIconService : IDisposable
	{
		private readonly IEnumerable<ContextMenuStripItem> _contextMenuItems;

		private readonly Texture2D _cornerIconTexture;

		private readonly Texture2D _cornerIconHoverTexture;

		private readonly Texture2D _cornerIconNotificationTexture;

		private readonly Texture2D _cornerIconNotificationHoverTexture;

		private readonly SettingEntry<bool> _cornerIconIsVisibleSetting;

		private readonly string _tooltip;

		private CornerIcon? _cornerIcon;

		private CornerIconTooltipView? _tooltipView;

		private bool _hasNotification;

		private string? _motdMessage;

		private string? _currentMotdId;

		public event EventHandler<bool>? IconLeftClicked;

		public CornerIconService(SettingEntry<bool> cornerIconIsVisibleSetting, string tooltip, Texture2D defaultTexture, Texture2D hoverTexture, Texture2D notificationTexture, Texture2D notificationHoverTexture, IEnumerable<ContextMenuStripItem> contextMenuItems)
		{
			_tooltip = tooltip;
			_cornerIconIsVisibleSetting = cornerIconIsVisibleSetting;
			_cornerIconTexture = defaultTexture;
			_cornerIconHoverTexture = hoverTexture;
			_cornerIconNotificationTexture = notificationTexture;
			_cornerIconNotificationHoverTexture = notificationHoverTexture;
			_contextMenuItems = contextMenuItems;
			_hasNotification = false;
			cornerIconIsVisibleSetting.add_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnCornerIconIsVisibleSettingChanged);
			Service.Settings.CornerIconPriority.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)CornerIconPriority_SettingChanged);
			if (cornerIconIsVisibleSetting.get_Value())
			{
				CreateCornerIcon();
			}
		}

		public void UpdateAccountName(string name)
		{
			if (_tooltipView != null)
			{
				_tooltipView!.UpdateAccountName(name);
			}
		}

		public void SetNotificationState(bool hasNotification, string? motdMessage = null)
		{
			_hasNotification = hasNotification;
			_motdMessage = motdMessage;
			if (_cornerIcon != null)
			{
				UpdateIconTextures();
				UpdateTooltip();
			}
			if (_tooltipView != null)
			{
				_tooltipView!.MotdMessage = motdMessage;
			}
		}

		private void UpdateIconTextures()
		{
			if (_cornerIcon != null)
			{
				if (_hasNotification)
				{
					_cornerIcon!.set_Icon(AsyncTexture2D.op_Implicit(_cornerIconNotificationTexture));
					_cornerIcon!.set_HoverIcon(AsyncTexture2D.op_Implicit(_cornerIconNotificationHoverTexture));
				}
				else
				{
					_cornerIcon!.set_Icon(AsyncTexture2D.op_Implicit(_cornerIconTexture));
					_cornerIcon!.set_HoverIcon(AsyncTexture2D.op_Implicit(_cornerIconHoverTexture));
				}
			}
		}

		private void UpdateTooltip()
		{
			if (_cornerIcon != null && _tooltipView != null)
			{
				((Control)_cornerIcon).set_BasicTooltipText((string)null);
				((Control)_cornerIcon).set_Tooltip((Tooltip)(object)_tooltipView);
				_tooltipView!.MotdMessage = _motdMessage;
				_tooltipView!.UpdateAccountName(Service.CurrentAccountName);
			}
		}

		public void Dispose()
		{
			_cornerIconIsVisibleSetting.remove_SettingChanged((EventHandler<ValueChangedEventArgs<bool>>)OnCornerIconIsVisibleSettingChanged);
			Service.Settings.CornerIconPriority.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)CornerIconPriority_SettingChanged);
			RemoveCornerIcon();
			CornerIconTooltipView? tooltipView = _tooltipView;
			if (tooltipView != null)
			{
				((Control)tooltipView).Dispose();
			}
		}

		private void CreateCornerIcon()
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Expected O, but got Unknown
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Expected O, but got Unknown
			RemoveCornerIcon();
			if (_tooltipView == null)
			{
				_tooltipView = new CornerIconTooltipView();
				_tooltipView!.UpdateAccountName(Service.CurrentAccountName);
				if (!string.IsNullOrEmpty(_motdMessage))
				{
					_tooltipView!.MotdMessage = _motdMessage;
				}
			}
			CornerIcon val = new CornerIcon();
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			val.set_Priority((int)(2.1474836E+09f * ((1000f - (float)Service.Settings.CornerIconPriority.get_Value()) / 1000f)) - 1);
			_cornerIcon = val;
			UpdateIconTextures();
			UpdateTooltip();
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)OnCornerIconClicked);
			((Control)_cornerIcon).add_MouseEntered((EventHandler<MouseEventArgs>)OnCornerIconMouseEntered);
			((Control)_cornerIcon).set_Menu(new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)(() => _contextMenuItems)));
		}

		private void RemoveCornerIcon()
		{
			if (_cornerIcon != null)
			{
				((Control)_cornerIcon).remove_Click((EventHandler<MouseEventArgs>)OnCornerIconClicked);
				((Control)_cornerIcon).remove_MouseEntered((EventHandler<MouseEventArgs>)OnCornerIconMouseEntered);
				((Control)_cornerIcon).Dispose();
			}
		}

		private void CornerIconPriority_SettingChanged(object sender, ValueChangedEventArgs<int> e)
		{
			if (Service.Settings.GlobalCornerIconEnabled.get_Value() && _cornerIcon != null)
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

		private void OnCornerIconMouseEntered(object sender, MouseEventArgs e)
		{
			if (_hasNotification)
			{
				_hasNotification = false;
				UpdateIconTextures();
				if (!string.IsNullOrEmpty(_currentMotdId) && Service.Settings != null)
				{
					Service.Settings.LastShownMotdId.set_Value(_currentMotdId);
				}
			}
		}

		public void SetCurrentMotdId(string? motdId)
		{
			_currentMotdId = motdId;
		}
	}
}

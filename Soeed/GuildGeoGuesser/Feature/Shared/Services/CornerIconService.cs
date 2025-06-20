using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Gw2Sharp.WebApi.V2.Models;

namespace Soeed.GuildGeoGuesser.Feature.Shared.Services
{
	public class CornerIconService : IDisposable
	{
		private readonly IEnumerable<ContextMenuStripItem> _contextMenuItems;

		private readonly AsyncTexture2D _cornerIconTexture;

		private readonly SettingEntry<bool> _cornerIconIsVisibleSetting;

		private readonly string _tooltip;

		private CornerIcon _cornerIcon;

		private string _accountName = "Unknown account";

		public event EventHandler<bool>? IconLeftClicked;

		public CornerIconService(string tooltip, AsyncTexture2D defaultTexture, IEnumerable<ContextMenuStripItem> contextMenuItems)
		{
			_tooltip = tooltip;
			_cornerIconTexture = defaultTexture;
			_contextMenuItems = contextMenuItems;
			Service.Settings.CornerIconPriority.add_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)CornerIconPriority_SettingChanged);
			Service.UserManager.AccountUpdated += new EventHandler<Account>(UserManager_AccountUpdated);
			CreateCornerIcon();
		}

		public void UpdateAccountName(string name)
		{
			_accountName = name;
			if (_cornerIcon != null)
			{
				((Control)_cornerIcon).set_BasicTooltipText(GetTooltipText());
			}
		}

		public void Dispose()
		{
			Service.Settings.CornerIconPriority.remove_SettingChanged((EventHandler<ValueChangedEventArgs<int>>)CornerIconPriority_SettingChanged);
			Service.UserManager.AccountUpdated -= new EventHandler<Account>(UserManager_AccountUpdated);
			RemoveCornerIcon();
		}

		private void CreateCornerIcon()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			RemoveCornerIcon();
			CornerIcon val = new CornerIcon();
			val.set_Icon(_cornerIconTexture);
			((Control)val).set_BasicTooltipText(GetTooltipText());
			((Control)val).set_Parent((Container)(object)GameService.Graphics.get_SpriteScreen());
			val.set_Priority((int)(2.1474836E+09f * ((1000f - (float)Service.Settings.CornerIconPriority.get_Value()) / 1000f)) - 1);
			_cornerIcon = val;
			((Control)_cornerIcon).add_Click((EventHandler<MouseEventArgs>)OnCornerIconClicked);
			((Control)_cornerIcon).set_Menu(new ContextMenuStrip((Func<IEnumerable<ContextMenuStripItem>>)(() => _contextMenuItems)));
		}

		private void UserManager_AccountUpdated(object sender, Account account)
		{
			_accountName = account.get_Name();
			if (_cornerIcon != null)
			{
				((Control)_cornerIcon).set_BasicTooltipText(GetTooltipText());
			}
		}

		private string GetTooltipText()
		{
			return _tooltip + "\n\n" + _accountName;
		}

		private void RemoveCornerIcon()
		{
			if (_cornerIcon != null)
			{
				((Control)_cornerIcon).remove_Click((EventHandler<MouseEventArgs>)OnCornerIconClicked);
				((Control)_cornerIcon).Dispose();
			}
		}

		private void CornerIconPriority_SettingChanged(object sender, ValueChangedEventArgs<int> e)
		{
			_cornerIcon.set_Priority((int)(2.1474836E+09f * ((1000f - (float)e.get_NewValue()) / 1000f)) - 1);
		}

		private void OnCornerIconClicked(object sender, MouseEventArgs e)
		{
			this.IconLeftClicked?.Invoke(this, e: true);
		}
	}
}

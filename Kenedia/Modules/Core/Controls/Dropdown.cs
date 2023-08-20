using System;
using System.Collections.Generic;
using System.Linq;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Services;

namespace Kenedia.Modules.Core.Controls
{
	public class Dropdown : Dropdown, ILocalizable
	{
		private Func<List<string>> _setLocalizedItems;

		private Func<string> _setLocalizedTooltip;

		public Func<List<string>> SetLocalizedItems
		{
			get
			{
				return _setLocalizedItems;
			}
			set
			{
				_setLocalizedItems = value;
				UserLocale_SettingChanged(null, null);
			}
		}

		public Func<string> SetLocalizedTooltip
		{
			get
			{
				return _setLocalizedTooltip;
			}
			set
			{
				_setLocalizedTooltip = value;
				((Control)this).set_BasicTooltipText(value?.Invoke());
			}
		}

		public Action<string> ValueChangedAction { get; set; }

		public Dropdown()
			: this()
		{
			LocalizingService.LocaleChanged += UserLocale_SettingChanged;
			UserLocale_SettingChanged(null, null);
		}

		public void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			if (SetLocalizedItems != null)
			{
				int? index = ((Dropdown)this).get_Items()?.ToList().FindIndex((string a) => a == ((Dropdown)this).get_SelectedItem());
				((Dropdown)this).get_Items().Clear();
				List<string> items = SetLocalizedItems?.Invoke();
				string selected = string.Empty;
				for (int i = 0; i < items.Count; i++)
				{
					string item = items[i];
					((Dropdown)this).get_Items().Add(item);
					if (index == i)
					{
						selected = item;
					}
				}
				((Dropdown)this).set_SelectedItem(selected ?? ((Dropdown)this).get_SelectedItem());
			}
			if (SetLocalizedTooltip != null)
			{
				((Control)this).set_BasicTooltipText(SetLocalizedTooltip?.Invoke());
			}
		}

		protected override void OnValueChanged(ValueChangedEventArgs e)
		{
			((Dropdown)this).OnValueChanged(e);
			ValueChangedAction?.Invoke(((Dropdown)this).get_SelectedItem());
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			GameService.Overlay.get_UserLocale().remove_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
		}
	}
}

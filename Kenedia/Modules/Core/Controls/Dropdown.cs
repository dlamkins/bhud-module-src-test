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
	public class Dropdown : Blish_HUD.Controls.Dropdown, ILocalizable
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
				base.BasicTooltipText = value?.Invoke();
			}
		}

		public Action<string> ValueChangedAction { get; set; }

		public Dropdown()
		{
			LocalizingService.LocaleChanged += new EventHandler<ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			UserLocale_SettingChanged(null, null);
		}

		public void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			if (SetLocalizedItems != null)
			{
				int? index = base.Items?.ToList().FindIndex((string a) => a == base.SelectedItem);
				base.Items.Clear();
				List<string> items = SetLocalizedItems?.Invoke();
				string selected = string.Empty;
				for (int i = 0; i < items.Count; i++)
				{
					string item = items[i];
					base.Items.Add(item);
					if (index == i)
					{
						selected = item;
					}
				}
				base.SelectedItem = selected ?? base.SelectedItem;
			}
			if (SetLocalizedTooltip != null)
			{
				base.BasicTooltipText = SetLocalizedTooltip?.Invoke();
			}
		}

		protected override void OnValueChanged(ValueChangedEventArgs e)
		{
			base.OnValueChanged(e);
			ValueChangedAction?.Invoke(base.SelectedItem);
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			GameService.Overlay.UserLocale.SettingChanged -= UserLocale_SettingChanged;
		}
	}
}

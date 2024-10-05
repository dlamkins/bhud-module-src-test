using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Services;

namespace Kenedia.Modules.Core.Controls
{
	public class ContextMenuItem : ContextMenuStripItem, ILocalizable
	{
		private Func<string> _setLocalizedText;

		private Func<string> _setLocalizedTooltip;

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

		public Func<string> SetLocalizedText
		{
			get
			{
				return _setLocalizedText;
			}
			set
			{
				_setLocalizedText = value;
				base.Text = value?.Invoke();
			}
		}

		public Action OnClickAction { get; set; }

		public ContextMenuItem()
		{
			LocalizingService.LocaleChanged += new EventHandler<ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			UserLocale_SettingChanged(null, null);
		}

		public ContextMenuItem(Func<string> setLocalizedText)
			: this()
		{
			SetLocalizedText = setLocalizedText;
		}

		public ContextMenuItem(Func<string> setLocalizedText, Action onClickAction)
			: this()
		{
			SetLocalizedText = setLocalizedText;
			OnClickAction = onClickAction;
		}

		public ContextMenuItem(Func<string> setLocalizedText, Action onClickAction, Func<string> setLocalizedTooltip = null)
			: this()
		{
			SetLocalizedText = setLocalizedText;
			OnClickAction = onClickAction;
			SetLocalizedTooltip = setLocalizedTooltip;
		}

		public void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			if (SetLocalizedText != null)
			{
				base.Text = SetLocalizedText?.Invoke();
			}
			if (SetLocalizedTooltip != null)
			{
				base.BasicTooltipText = SetLocalizedTooltip?.Invoke();
			}
		}

		protected override void OnClick(MouseEventArgs e)
		{
			base.OnClick(e);
			OnClickAction?.Invoke();
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			LocalizingService.LocaleChanged -= new EventHandler<ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
		}
	}
}

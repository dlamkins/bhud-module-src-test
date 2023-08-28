using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Services;

namespace Kenedia.Modules.Core.Controls
{
	public class TextBox : TextBox, ILocalizable
	{
		private Func<string> _setLocalizedText;

		private Func<string> _setLocalizedTooltip;

		private Func<string> _setLocalizedPlaceholder;

		public Func<string> SetLocalizedText
		{
			get
			{
				return _setLocalizedText;
			}
			set
			{
				_setLocalizedText = value;
				((TextInputBase)this).set_Text(value?.Invoke());
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

		public Func<string> SetLocalizedPlaceholder
		{
			get
			{
				return _setLocalizedPlaceholder;
			}
			set
			{
				_setLocalizedPlaceholder = value;
				((TextInputBase)this).set_PlaceholderText(value?.Invoke());
			}
		}

		public Action ClickAction { get; set; }

		public Action<string> EnterPressedAction { get; set; }

		public Action<string> TextChangedAction { get; set; }

		public TextBox()
			: this()
		{
			LocalizingService.LocaleChanged += UserLocale_SettingChanged;
			((TextInputBase)this).add_TextChanged((EventHandler<EventArgs>)OnTextChanged);
			UserLocale_SettingChanged(null, null);
		}

		public void ResetText()
		{
			((TextInputBase)this).set_Text((string)null);
		}

		protected override void OnEnterPressed(EventArgs e)
		{
			((TextBox)this).OnEnterPressed(e);
			EnterPressedAction?.Invoke(((TextInputBase)this).get_Text());
		}

		public void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			if (SetLocalizedText != null)
			{
				((TextInputBase)this).set_Text(SetLocalizedText?.Invoke());
			}
			if (SetLocalizedTooltip != null)
			{
				((Control)this).set_BasicTooltipText(SetLocalizedTooltip?.Invoke());
			}
			if (SetLocalizedPlaceholder != null)
			{
				((TextInputBase)this).set_PlaceholderText(SetLocalizedPlaceholder?.Invoke());
			}
		}

		protected override void DisposeControl()
		{
			((TextInputBase)this).DisposeControl();
			((TextInputBase)this).remove_TextChanged((EventHandler<EventArgs>)OnTextChanged);
			GameService.Overlay.get_UserLocale().remove_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
		}

		private void OnTextChanged(object sender, EventArgs e)
		{
			TextChangedAction?.Invoke(((TextInputBase)this).get_Text());
		}
	}
}

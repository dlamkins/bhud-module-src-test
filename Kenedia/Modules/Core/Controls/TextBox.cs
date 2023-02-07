using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;

namespace Kenedia.Modules.Core.Controls
{
	public class TextBox : TextBox, ILocalizable
	{
		private Func<string> _setLocalizedText;

		private Func<string> _setLocalizedTooltip;

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

		public Action ClickAction { get; set; }

		public Action<string> EnterPressedAction { get; set; }

		public Action<string> TextChangedAction { get; set; }

		public TextBox()
			: this()
		{
			GameService.Overlay.get_UserLocale().add_SettingChanged((EventHandler<ValueChangedEventArgs<Locale>>)UserLocale_SettingChanged);
			((TextInputBase)this).add_TextChanged((EventHandler<EventArgs>)OnTextChanged);
			UserLocale_SettingChanged(null, null);
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

using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Services;

namespace Kenedia.Modules.Core.Controls
{
	public class TextBox : Blish_HUD.Controls.TextBox, ILocalizable
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
				base.Text = value?.Invoke();
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

		public Func<string> SetLocalizedPlaceholder
		{
			get
			{
				return _setLocalizedPlaceholder;
			}
			set
			{
				_setLocalizedPlaceholder = value;
				base.PlaceholderText = value?.Invoke();
			}
		}

		public Action ClickAction { get; set; }

		public Action<string> EnterPressedAction { get; set; }

		public Action<string> TextChangedAction { get; set; }

		public TextBox()
		{
			LocalizingService.LocaleChanged += new EventHandler<ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			base.TextChanged += OnTextChanged;
			UserLocale_SettingChanged(null, null);
		}

		public void ResetText()
		{
			base.Text = null;
		}

		protected override void OnEnterPressed(EventArgs e)
		{
			base.OnEnterPressed(e);
			EnterPressedAction?.Invoke(base.Text);
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
			if (SetLocalizedPlaceholder != null)
			{
				base.PlaceholderText = SetLocalizedPlaceholder?.Invoke();
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			base.TextChanged -= OnTextChanged;
			GameService.Overlay.UserLocale.SettingChanged -= UserLocale_SettingChanged;
		}

		private void OnTextChanged(object sender, EventArgs e)
		{
			TextChangedAction?.Invoke(base.Text);
		}

		protected override void OnClick(MouseEventArgs e)
		{
			if (base.Enabled)
			{
				base.OnClick(e);
			}
		}

		protected override CaptureType CapturesInput()
		{
			if (!base.Enabled)
			{
				return CaptureType.None;
			}
			return base.CapturesInput();
		}
	}
}

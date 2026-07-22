using System;
using System.Runtime.CompilerServices;
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
		public Func<string> SetLocalizedText
		{
			[CompilerGenerated]
			get
			{
				return _003CSetLocalizedText_003Ek__BackingField;
			}
			set
			{
				_003CSetLocalizedText_003Ek__BackingField = value;
				base.Text = value?.Invoke();
			}
		}

		public Func<string> SetLocalizedTooltip
		{
			[CompilerGenerated]
			get
			{
				return _003CSetLocalizedTooltip_003Ek__BackingField;
			}
			set
			{
				_003CSetLocalizedTooltip_003Ek__BackingField = value;
				base.BasicTooltipText = value?.Invoke();
			}
		}

		public Func<string> SetLocalizedPlaceholder
		{
			[CompilerGenerated]
			get
			{
				return _003CSetLocalizedPlaceholder_003Ek__BackingField;
			}
			set
			{
				_003CSetLocalizedPlaceholder_003Ek__BackingField = value;
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

		protected virtual void OnTextChanged(object sender, EventArgs e)
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

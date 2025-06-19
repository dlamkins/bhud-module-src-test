using System;
using System.ComponentModel;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;

namespace Kenedia.Modules.Core.Controls
{
	public class Image : Blish_HUD.Controls.Image, ILocalizable
	{
		private Func<string> _setLocalizedTooltip;

		private Color? _defaultBackgroundColor;

		private Color? _hoveredBackgroundColor;

		public Color? HoveredBackgroundColor
		{
			get
			{
				return _hoveredBackgroundColor;
			}
			set
			{
				Common.SetProperty(ref _hoveredBackgroundColor, value);
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

		public Image()
		{
			LocalizingService.LocaleChanged += new EventHandler<ValueChangedEventArgs<Locale>>(UserLocale_SettingChanged);
			UserLocale_SettingChanged(null, null);
			base.PropertyChanged += OnPropertyChanged;
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			if (e.PropertyName == "BackgroundColor")
			{
				Color backgroundColor = base.BackgroundColor;
				Color? hoveredBackgroundColor = _hoveredBackgroundColor;
				if (!hoveredBackgroundColor.HasValue || backgroundColor != hoveredBackgroundColor.GetValueOrDefault())
				{
					_defaultBackgroundColor = base.BackgroundColor;
				}
			}
		}

		public void UserLocale_SettingChanged(object sender, ValueChangedEventArgs<Locale> e)
		{
			if (SetLocalizedTooltip != null)
			{
				base.BasicTooltipText = SetLocalizedTooltip?.Invoke();
			}
		}

		protected override void DisposeControl()
		{
			base.DisposeControl();
			GameService.Overlay.UserLocale.SettingChanged -= UserLocale_SettingChanged;
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			base.OnMouseEntered(e);
			base.BackgroundColor = (Color)(((_003F?)_hoveredBackgroundColor) ?? base.BackgroundColor);
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			base.OnMouseLeft(e);
			base.BackgroundColor = (Color)(((_003F?)_defaultBackgroundColor) ?? base.BackgroundColor);
		}
	}
}

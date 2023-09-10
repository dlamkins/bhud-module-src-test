using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Gw2Sharp.WebApi;
using Kenedia.Modules.Core.Interfaces;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Services;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kenedia.Modules.Core.Controls
{
	public class NotificationBadge : Control, ILocalizable
	{
		private readonly DetailedTexture _badge = new DetailedTexture(222246);

		private Func<string> _setLocalizedText;

		private Control _anchor;

		private float _opcacity = 1f;

		public CaptureType? CaptureInput { get; set; }

		public Func<string> SetLocalizedText
		{
			get
			{
				return _setLocalizedText;
			}
			set
			{
				Common.SetProperty(ref _setLocalizedText, value, (Action)delegate
				{
					UserLocale_SettingChanged();
				}, triggerOnUpdate: true);
			}
		}

		public float HoveredOpacity { get; set; } = 1f;


		public float Opacity
		{
			get
			{
				return _opcacity;
			}
			set
			{
				_opcacity = value;
				SetOpacity();
			}
		}

		public Control Anchor
		{
			get
			{
				return _anchor;
			}
			set
			{
				Common.SetProperty(ref _anchor, value, OnAnchorChanged);
			}
		}

		public Func<string> SetLocalizedTooltip { get; set; }

		public Action ClickAction { get; set; }

		public NotificationBadge()
			: this()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Size(new Point(32));
			LocalizingService.LocaleChanged += UserLocale_SettingChanged;
		}

		private void OnAnchorChanged(object sender, ValueChangedEventArgs<Control> e)
		{
			if (e.OldValue != null)
			{
				e.OldValue!.remove_MouseEntered((EventHandler<MouseEventArgs>)SetHoveredOpacity);
				e.OldValue!.remove_MouseLeft((EventHandler<MouseEventArgs>)SetOpacity);
			}
			if (e.NewValue != null)
			{
				e.NewValue!.add_MouseEntered((EventHandler<MouseEventArgs>)SetHoveredOpacity);
				e.NewValue!.add_MouseLeft((EventHandler<MouseEventArgs>)SetOpacity);
			}
		}

		private void SetOpacity(object sender = null, EventArgs e = null)
		{
			((Control)this).set_Opacity(Opacity);
		}

		private void SetHoveredOpacity(object sender = null, EventArgs e = null)
		{
			((Control)this).set_Opacity(HoveredOpacity);
		}

		public override void RecalculateLayout()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).RecalculateLayout();
			_badge.Bounds = new Rectangle(Point.get_Zero(), ((Control)this).get_Size());
		}

		public void UserLocale_SettingChanged(object sender = null, ValueChangedEventArgs<Locale> e = null)
		{
			((Control)this).set_BasicTooltipText(SetLocalizedText?.Invoke());
		}

		protected override void OnMouseMoved(MouseEventArgs e)
		{
			((Control)this).OnMouseMoved(e);
			SetHoveredOpacity();
		}

		protected override void OnMouseLeft(MouseEventArgs e)
		{
			((Control)this).OnMouseLeft(e);
			SetOpacity();
		}

		protected override void OnMouseEntered(MouseEventArgs e)
		{
			((Control)this).OnMouseEntered(e);
			SetHoveredOpacity();
		}

		protected override void OnClick(MouseEventArgs e)
		{
			((Control)this).OnClick(e);
			ClickAction?.Invoke();
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			_badge.Draw((Control)(object)this, spriteBatch);
		}

		protected override void DisposeControl()
		{
			((Control)this).DisposeControl();
			Anchor = null;
			LocalizingService.LocaleChanged -= UserLocale_SettingChanged;
		}

		protected override CaptureType CapturesInput()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_MouseOver())
			{
				return ((Control)this).CapturesInput();
			}
			return (CaptureType)(((_003F?)CaptureInput) ?? ((Control)this).CapturesInput());
		}
	}
}

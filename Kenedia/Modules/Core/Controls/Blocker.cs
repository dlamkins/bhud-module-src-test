using System;
using System.Collections.Generic;
using Blish_HUD;
using Blish_HUD.Controls;
using Kenedia.Modules.Core.Extensions;
using Kenedia.Modules.Core.Models;
using Kenedia.Modules.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace Kenedia.Modules.Core.Controls
{
	public class Blocker : Control
	{
		private List<(Rectangle r, float opacity, int thickness)> _borders = new List<(Rectangle, float, int)>();

		private Control _coveredControl;

		public Control CoveredControl
		{
			get
			{
				return _coveredControl;
			}
			set
			{
				Common.SetProperty(ref _coveredControl, value, new ValueChangedEventHandler<Control>(OnCoveredControlChanged));
			}
		}

		public int BorderWidth { get; set; } = 3;


		public string Text { get; set; } = string.Empty;


		public Color TextColor { get; set; } = Color.get_White();


		public BitmapFont TextFont { get; set; } = Control.Content.DefaultFont18;


		private void OnCoveredControlChanged(object sender, Kenedia.Modules.Core.Models.ValueChangedEventArgs<Control> e)
		{
			if (e.OldValue != null)
			{
				e.OldValue!.Resized -= CoveredControl_Resized;
			}
			if (e.NewValue != null)
			{
				e.NewValue!.Resized += CoveredControl_Resized;
			}
		}

		private void CoveredControl_Resized(object sender, ResizedEventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			base.Size = e.CurrentSize;
		}

		public override void RecalculateLayout()
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			base.RecalculateLayout();
			_borders.Clear();
			ZIndex = (base.Parent?.ZIndex ?? 0) + 25;
			int strength = BorderWidth;
			int fadeLines = Math.Max(0, Math.Min(strength - 1, 4));
			for (int i = fadeLines - 1; i >= 0; i--)
			{
				_borders.Add((new Rectangle(i, i, base.Width - i * 2, base.Height - i * 2), GetFadeValue(i), 1));
			}
			if (fadeLines < strength)
			{
				_borders.Add((new Rectangle(fadeLines, fadeLines, base.Width - fadeLines * 2, base.Height - fadeLines * 2), GetFadeValue(int.MaxValue), strength - fadeLines));
			}
		}

		private float GetFadeValue(int i)
		{
			return i switch
			{
				2 => 0.75f, 
				1 => 0.5f, 
				0 => 0.25f, 
				_ => 1f, 
			};
		}

		private float GetFadeValue(int i, int fadeLines, int strength)
		{
			return (float)strength / (float)fadeLines * (float)i;
		}

		protected override void Paint(SpriteBatch spriteBatch, Rectangle bounds)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			foreach (var (r, opacity, thickness) in _borders)
			{
				spriteBatch.DrawFrame(this, r, Color.get_Black() * opacity, thickness);
			}
			if (!string.IsNullOrEmpty(Text))
			{
				spriteBatch.DrawStringOnCtrl(this, Text, TextFont, bounds, TextColor, wrap: false, HorizontalAlignment.Center);
			}
		}
	}
}
